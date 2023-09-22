import { navigate } from "svelte-routing"
import {HttpResponse, get, pollingGet, post } from '@src/services/backend/http'
import {get as getFromStore} from 'svelte/store'
import { userStore } from '@src/helpers/stores'
import {auth} from '@src/helpers/firebase.js'
import type { Chat, Identity } from "@src/models/models"

export const logOut = () => {
    localStorage.clear()
    userStore.set(null)
    navigate('/login')
}



export const deleteAccount = (userId: string): Promise<HttpResponse<boolean>> => {
    return get<boolean>(`/deleteAccount/${userId}`)
}

export const exportAccount = async () => {
    let blob = await get<string>(`/exportAccount/${getFromStore(userStore).user.id}`, true)
        // Trigger the download on the frontend
        const url = window.URL.createObjectURL(new Blob([blob.obj]));
        const link = document.createElement('a');
        link.href = url;
        link.setAttribute('download', 'user_data.pdf');
        document.body.appendChild(link);
        link.click();
        link.remove();
}


