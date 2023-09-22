<script lang="ts">
  import { Role } from '@src/models/models'
	import { userStore } from './../../helpers/stores';
    import { auth } from '@src/helpers/firebase'
  import { getIdentity } from '@src/services/backend/api'
  import { onMount } from 'svelte'
      import {Card, CardBody, CardHeader, CardTitle, Col, Container, Row} from 'sveltestrap'
  import { navigate } from 'svelte-routing'
  import { logOut } from '@src/helpers/usersManagement'

      onMount(async () => {
        let {obj: identity} = await getIdentity($userStore.user.id)
        if(identity.role == Role.Doctor) navigate('/doctor-home')
        else if(identity.role == Role.Patient) navigate('/patient-home')
        //else we do nothing
      })
  </script>
 
  <Card style="margin-top: 4rem;  background-color: lightblue; width: 40%">
      <CardHeader>
          <CardTitle> {auth.currentUser?.displayName ?? "N/A"}</CardTitle>
      </CardHeader>
      <CardBody style=" background-color: lightgrey;">
          <p>Waiting for confirmation of identity, please login later! </p>
      </CardBody>
  </Card>
  <button on:click={logOut}>Log out</button>


 


