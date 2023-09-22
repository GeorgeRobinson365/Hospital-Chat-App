<script lang="ts">
  import {
    User,
    UserCredential,
    createUserWithEmailAndPassword,
    updateProfile,
  } from 'firebase/auth'
  import { getAuth, signInWithEmailAndPassword } from 'firebase/auth'
  import { sendPasswordResetEmail } from 'firebase/auth'
  import { auth } from '@src/helpers/firebase'
  import { userStore } from '@src/helpers/stores'
  import { CreateIdentityCommand, Identity, IdentityStore, Role } from '@src/models/models'
  import { navigate } from 'svelte-routing'
  import { CloudEvent, HttpResponse, pollingGetCondition, post } from '@src/services/backend/http'
  import type { type } from 'os'
  import { Card } from 'sveltestrap'
  import { Notification } from '@vaadin/notification'
  import { getIdentity } from '@src/services/backend/api'
  import { get as getFromStore } from 'svelte/store'
  let email: string = ''
  let password: string = ''
  let role: number = 0
  let user: User
  let errorMessage = ''
  let Fullname = ''

  const createUser = (): Promise<Identity> => {
    return createUserWithEmailAndPassword(auth, email, password).then((userCredential) => {
      user = userCredential.user
      return userCredential.user.getIdToken().then(async (idToken) => {
        const identity: IdentityStore = {
          fbToken: idToken,
        }
        userStore.set(identity)
        let createIdentityCommand: CreateIdentityCommand = {
          id: user.uid,
          role: role,
          fullname: Fullname,
        }
        await updateProfile(user, { displayName: Fullname })
        return post('/createidentity', createIdentityCommand)
          .then((response) => {
            console.log(response)
            return pollingGetCondition<Identity>(`/getidentity/${user.uid}`, (response) => {
              return response.status === 200
            })
          })
          .then((res) => {
            userStore.update((identity) => {
              identity.user = res.obj
              return identity
            })
            localStorage.setItem('user', JSON.stringify({
              user: res.obj,
              fbToken: idToken
            })
            )
            console.log(res.obj.role)
            console.log(Role.Admin)
            navigate('/confirmation')
            return res.obj
          })
      })
    })
  }

  const signIn = () => {
    return signInWithEmailAndPassword(auth, email, password)
      .then((userCredential) => {
        user = userCredential.user
        return user.getIdToken()
      })
      .then((fbToken) => {
        const identity: IdentityStore = {
          fbToken: fbToken,
        }
        userStore.set(identity)
        return getIdentity(user.uid)
      })
      .then((resp) => {
        let retrievedIdentiy = resp.obj
        userStore.update((identity) => {
          identity.user = retrievedIdentiy
          return identity
        })
        console.log($userStore)
        localStorage.setItem('user', JSON.stringify($userStore))
        console.log(resp.obj.role)
        console.log(Role.Admin)
        let {role} = retrievedIdentiy
        if(role == Role.PendingDoctor || role == Role.PendingPatient) navigate('/confirmation')
        if (role == Role.Admin) navigate('/admin')
        else if(role==Role.Doctor) navigate('/doctor-home')
        else navigate('/patient-home')
        return retrievedIdentiy
      })
      .catch((error) => {
        const errorCode = error.code
        let errorMessage = error.message
        Notification.show(errorMessage, {
          theme: 'error',
        })
        console.log(errorMessage)
      })
  }
  const getAuth = () => {
    sendPasswordResetEmail(auth, email)
      .then(() => {})
      .catch((error) => {
        const errorCode = error.code
        const errorMessage = error.message
      })
  }
</script>

<Card>
  <h1>Create account: <br /></h1>
  <b> Full name: </b>
  <input type="text" bind:value={Fullname} />
  <br />
  <b>Email: </b>
  <input type="email" bind:value={email} />
  <br />
  <b> Password: </b>
  <input type="password" bind:value={password} />
  <br />
  <b> Account type: </b>
  <select bind:value={role}>
    <option value="2">Doctor</option>
    <option value="3">Patient</option>
  </select>
  <br />
  <button on:click={createUser}>Create User</button>
  <p class="error pink-text center-align" />
  <b />
</Card>
<Card>
  <h1>Login to account: <br /></h1>
  <b>Email: </b>
  <input type="email" bind:value={email} />
  <br />
  <b> Password: </b>
  <input type="password" bind:value={password} />
  <br />
  <button on:click={signIn}>Login User</button>
  <br />
  <button on:click={getAuth}>Reset password</button>
  <p class="error pink-text center-align" />
  <p> {errorMessage} </p>
</Card>

<style>
  :global(.card) {
    margin: 30px;
    border-radius: 16px;
    padding: 15px;
    align-items: center;
    display: flex;
  }
  :global(.card button) {
    background-color: lightblue;
    border-radius: 16px;
    width: 80%;
  }
  :global(.card input, .card select) {
    width: 80%;
  }
</style>
