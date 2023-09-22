export type Identity = {
    id: string
    role: Role
    apiToken: string
    fullName : string
    patients: string[]
}

export type Patient = {
    id: string
    email: string
    fullName : string
    doctor?: string
}

export type IdentityStore = {
    fbToken: string 
    user?: Identity
}

export enum Role {
    Doctor,
    Patient,
    PendingDoctor,
    PendingPatient,
    Admin
}

export type CreateIdentityCommand = {
    id: string
    role: Role
    fullname : string
}

export type SubmitPatientAccessComand = {
    DoctorId: string
    PatientId: string
}

export type CreateChatCommand = {
    participantId: [string, string]    
}

export type SendMessageCommand = {
    senderId: string
    chatId: string
    content: string
}


export type Medicine = {
    brandName: string
}

export type PrescriptionRequest = {
    medicine: Medicine
    dosage: string
    lastTaken: Date
    frequency: string
}

export type Doctor = {
    id: string
    fullName: string
    patients: string[]
}


//Chat

export type Chat = {
    participantId: [string, string]
    id: string
    chatId: string
    name: string
}

export type Message = {
    senderId: string
    content: string
    sentAt: Date
    id: string
    chatid: string
    chatId: string
}