import { v4 as uuid } from 'uuid'
import log from 'loglevel'

import {HttpResponse, get, pollingGet, post } from '@src/services/backend/http'
import type { CloudEvent, Command } from '@src/services/backend/http'
import type { Chat, CreateChatCommand, Doctor, Identity, Message, Patient, SendMessageCommand, SubmitPatientAccessComand } from '@src/models/models'

export const toCloudEvent = <T>(cmd: Command<T>): CloudEvent<T> => {
  const ce: CloudEvent<T> = {
    ...cmd,
    id: uuid(),
    time: new Date(),
    dataContentType: 'application/json',
  }
  return ce
}

export const getIdentity = (userId: string): Promise<HttpResponse<Identity>> => {
  return get<Identity>(`/getidentity/${userId}`)
}

export const getPendingIdentities = (): Promise<HttpResponse<Identity[]>> => {
  return get<Identity[]>(`/identities/pending`)
}

export const getPatients = (): Promise<HttpResponse<Patient[]>> => {
  return get<Patient[]>(`/patients`)
}

export const getDoctors = (): Promise<HttpResponse<Doctor[]>> => {
  return get<Doctor[]>(`/doctors`)
}

export const getDoctor = (patientId: string): Promise<HttpResponse<Doctor>> => {
  return get<Doctor>(`/doctors/${patientId}`)
}

export const submitApproval = (userId: string) : Promise<HttpResponse<Identity[]>> => {
  return get<Identity[]>(`/submitapproval/${userId}`)
}

export const submitPatientAccess = (submitPatientAccessComand: SubmitPatientAccessComand) : Promise<HttpResponse<Identity[]>> => {
  return post(`/submitPatientAccess`, submitPatientAccessComand)
}

export const createChat = (createChatCommand: CreateChatCommand) : Promise<HttpResponse<Chat>> => {
  return post(`/createchat`, createChatCommand)
}

export const sendMessage = (sendMessage: SendMessageCommand) : Promise<HttpResponse<Message>> => {
  return post(`/sendMessage`, sendMessage)
}

export const getChats = (userId: string): Promise<HttpResponse<Chat[]>> => {
  return get<Chat[]>(`/chats/${userId}`)
}

export const getMessages = (id: string): Promise<HttpResponse<Message[]>> => {
  return get<Message[]>(`/messages/${id}`)
}