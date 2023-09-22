<script lang="ts">
  import {Router, Route} from 'svelte-routing'
  import {addMessages, init} from 'svelte-i18n'
  import {get as getFromStore} from 'svelte/store'
  import log from 'loglevel'

  import ProtectedRoute from '@src/routes/ProtectedRoute.svelte'
  import '@src/vaadin-overrides.css'

  export let url: string = ''
  import data from '@src/routes/index'
  import { userStore } from './helpers/stores'
  import type { Identity, IdentityStore } from './models/models'
  import { auth, isUserExpired } from './helpers/firebase'
  import { onMount } from 'svelte'

  let userExists: boolean = false
  let loading: boolean = true

  init({
    initialLocale: 'en',
    fallbackLocale: 'en'
  })

  if (localStorage.getItem('user')) {
      userStore.set(JSON.parse(localStorage.getItem('user')))    
    userExists = true
  } else {
    log.info('Did not find user details in local storage.')
  }
  
  onMount(() => {
    isUserExpired()
  })

  userStore.subscribe((user: IdentityStore) => {
    if (user) {
      userExists = true
      loading = false
    } else {
      userExists = false
    }
  })

</script>

<svelte:head>
  <meta name="robots" content={"noindex"}/>

</svelte:head>

{#if loading}
<div/>
{:else}
    {#key userStore}
      <Router {url}>
        {#each data.publicRoutes as route}
          <Route path={route.path} let:params>
            <svelte:component this={route.component} />
          </Route>
        {/each}
        {#if userExists}
          {#each data.authProtectedRoutes as route}
            <Route path={route.path} let:params>
              <ProtectedRoute {route}>
                    <svelte:component this={route.component} {params} />
              </ProtectedRoute>
            </Route>
          {/each}
        {/if}
      </Router>
    {/key}
{/if}