import type { IdentityStore} from "@src/models/models";
import { Subscriber, Writable, writable } from "svelte/store";

const createStore = <T> (init?: T) : Writable<T> & {add: Function, remove: Function} => {
    const store = writable<T>(init ?? null)
    const {update} = store
    const add = (id: string, itemToAdd: T) => update((value) => {
        value[id] = itemToAdd
        return value
    })
    const remove = (id: string) => update((value) => {
        delete value[id]
        return value
    })
    return {...store, add, remove}
}

export const userStore = createStore<IdentityStore>({
    fbToken: null,
    user: {
        id: null,
        role: null,
        apiToken: null
    }
})