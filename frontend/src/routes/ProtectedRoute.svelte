<script lang="ts">
	import AdminNavbar from './AdminNavbar.svelte';
  import { IdentityStore, Role, Identity } from '@src/models/models'
  import log from 'loglevel'
  import {onMount} from 'svelte'
  import {navigate} from 'svelte-routing'
  import {get as getFromStore} from 'svelte/store'
  import {userStore} from '@src/helpers/stores'
  import DoctorHeader from './DoctorNavbar.svelte'
  import PatientNavbar from './PatientNavbar.svelte'

  // import Unauthorized from '@src/pages/Authentication/Unauthorized.svelte'

  export let route = {} as any

  let unauthorized: boolean = false
  let isPatient: boolean = false
  let isAdmin: boolean = false

  onMount(() => {
    authorize()
  })

  const authorize = (): void => {
    const storedUser: IdentityStore = getFromStore(userStore)
    const {user} = storedUser
    if (!storedUser) {
      unauthorized = true
      localStorage.clear()
    } else if (route.auth) {
      const validPermissionsToAccessRoute = route.auth
      console.log(route)
      const userPermissions = user.role
      console.log(userPermissions, validPermissionsToAccessRoute)
      const innerJoin: Role[] = validPermissionsToAccessRoute.validRoles.filter((role) => role == userPermissions)
      unauthorized = innerJoin.length === 0
      isPatient = user.role == Role.Patient
      isAdmin = user.role == Role.Admin
    }
  }
</script>

{#if unauthorized}
  {navigate('/login')}
{:else}
  {#if isPatient}
    <PatientNavbar />
  {:else if isAdmin}
    <AdminNavbar />
  {:else}
    <DoctorHeader />
  {/if}
  <slot />
{/if}
