import { userStore } from '@src/helpers/stores'
import type { IdentityStore } from '@src/models/models'
import log from 'loglevel'
import { get as getFromStore } from 'svelte/store'

export type Command<T> = {
  type: string
  source: string
  dataSchema: string
  data: T
}

export type CloudEvent<T> = {
  id: string
  time: Date
  dataContentType: string
  data: T
}

export type HttpResponse<T> = Response & {
  contentType: string
  isJson: boolean
  obj: T
  applies?: Date
}

export type Resource = string

export const getUrl = (path: string): string => {
  const url = "http://localhost:7071/api" + path
  log.debug(`URL: ${url}`)
  return url
}

export const delay = (ms: number) => {
  log.debug(`Delaying by ${ms} ms...`)
  return new Promise((resolve) => setTimeout(resolve, ms))
}

const initPost: RequestInit = {
  method: 'POST',
  headers: new Headers({
    'Content-Type': 'application/json',
    'Access-Control-Allow-Origin': '*',
    'Access-Control-Allow-Methods': 'GET, POST, PUT, DELETE, OPTIONS',
  }),
}

const initGet: RequestInit = {
  method: 'GET',
}

export const pathOf = (res: Resource): string => {
  return res.toString()
}

export const buildHeaders = (res: Resource, isPdf?: boolean): Headers => {
  log.debug(`Building headers for ${res}...`)
  const path = pathOf(res)
  const storedUser = getFromStore(userStore) as IdentityStore
  const token: string = path.includes("identity")
    ? storedUser.fbToken
    : storedUser?.user?.apiToken
  if (!token) log.warn('No token found!')
    const headers = new Headers({
    JWT: `Bearer ${token}`,
    'Content-Type': ` ${isPdf ? 'application/pdf' : 'application/json'}`,
    'Access-Control-Allow-Origin': '*',
    'Access-Control-Allow-Methods': 'GET, POST, PUT, DELETE, OPTIONS',
  })
  log.debug(`Built headers: `, headers)
  return headers
}

export const saveUser = (updatedUser: any) => {
  log.debug(`Saving user: ${updatedUser}`)

  log.debug(`Setting 'user' in svelte/store: ${updatedUser}`)
  userStore.set(updatedUser)

  log.debug(`Setting 'user' in localStorage: ${updatedUser}`)
  localStorage.removeItem('user')
  localStorage.setItem('user', JSON.stringify(getFromStore(userStore)))
}

export const wrapResponse = async <T>(
  r: Response,
  ce: CloudEvent<any> = null,
): Promise<HttpResponse<T>> => {
  log.debug(`Response: `, r)
  const contentType = r.headers.get('Content-Type')
  const isJson = contentType && contentType.indexOf('application/json') > -1
  var obj : T = isJson ? (await r.json() as Object) as T : null
  const wrapped: HttpResponse<T> = {
    applies: ce?.time,
    arrayBuffer: r.arrayBuffer,
    blob: r.blob,
    body: r.body,
    bodyUsed: r.bodyUsed,
    clone: r.clone,
    contentType: contentType,
    formData: r.formData,
    headers: r.headers,
    isJson: isJson,
    json: r.json,
    obj: obj,
    ok: r.ok,
    redirected: r.redirected,
    status: r.status,
    statusText: r.statusText,
    text: r.text,
    type: r.type,
    url: r.url,
  }
  log.debug(`HttpResponse: `, wrapped)
  return wrapped
}

const buildCloudEvent = <T>(command: T): CloudEvent<T> => {
  return {
    data: command,
    dataContentType: 'application/json',
    time: new Date(),
    id: '123',
  }
}

const maxAttempts = 30

export const post = async <T, U>(url: Resource, command: T): Promise<HttpResponse<U>> => {
  let ce = buildCloudEvent(command)
  const init = { ...initPost, body: JSON.stringify(ce) }
  let newUrl = getUrl(url)
  init.headers = buildHeaders(newUrl)
  log.debug(`Posting: ${url} : `, init)
  init.mode = 'cors'
  const response: HttpResponse<any> = await fetch(newUrl, init).then((x) => {
    return wrapResponse<U>(x, ce)
  })
  if (response.ok) {
    log.debug(`Response is ok: ${response.status} ${response.statusText}`)
    return Promise.resolve(response)
  } else {
    log.debug(`Request failed: ${response.status} ${response.statusText}`)
    return Promise.reject(response)
  }
}

export const get = async <T>(url: Resource, isPdf: boolean = false): Promise<HttpResponse<T>> => {
  const fullUrl: string = getUrl(url)
  const headers: Headers = buildHeaders(fullUrl, isPdf)
  const init: RequestInit = {
    ...initGet,
    headers: headers,
  }

  log.debug(`Getting: ${fullUrl} : `, init)
  const response: HttpResponse<T> = await fetch(fullUrl, init).then((x) => {
    return wrapResponse<T>(x)
  })

  if (response.ok) {
    log.debug(`Response is ok: ${response.status} ${response.statusText}`)
    return Promise.resolve(response)
  } else {
    log.debug(`Request failed: ${response.status} ${response.statusText}`)
    return Promise.reject(response)
  }
}

/**
 * Use this to poll a url until if you don't wish to check against a condition is met.
 * If you want to poll forever, pass true for pollForever.
 * @param url The url to poll.
 * @param pollForever If true, will poll forever, otherwise will stop after maxAttempts. Defaults to false.
 */
export const pollingGet = async <T>(
  url: Resource,
  pollForever: boolean = false,
): Promise<HttpResponse<T>> => {
  log.debug(`Polling get: ${url}`)
  return pollingGetCondition(url, (response: HttpResponse<T>) => true, pollForever)
}

/**
 * Use this to poll a url until a condition is met. If you want to poll forever, pass true for pollForever.
 * If you do not wish to use a condition, use pollingGet instead.
 * @param url The url to poll.
 * @param condition A function that returns true if the condition is met.
 * @param pollForever  If true, will poll forever, otherwise will stop after maxAttempts. Defaults to false.
 * @param attempt Do not pass this in, it is used internally to track the number of attempts.
 */
export const pollingGetCondition = async <T>(
  url: Resource,
  condition: (response: HttpResponse<T>) => boolean,
  pollForever: boolean = false,
  attempt: number = 1,
): Promise<HttpResponse<T>> => {
  if (!pollForever && attempt >= maxAttempts) {
    return Promise.reject(`Max attempts reached, ${attempt} of ${maxAttempts}.`)
  }
  if (pollForever) {
    Promise.reject(`Slow response from api`)
  }
  return get<T>(url)
    .then((response) => {
      if (condition(response)) {
        return Promise.resolve(response)
      } else {
        return Promise.reject('Condition not met.')
      }
    })
    .catch(async () => {
      await delay(500)
      return pollingGetCondition<T>(url, condition, pollForever, attempt + 1)
    })
}
