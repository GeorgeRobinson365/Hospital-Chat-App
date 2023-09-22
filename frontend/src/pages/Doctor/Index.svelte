<script lang="ts">
	import { userStore } from './../../helpers/stores';
    import { auth } from '@src/helpers/firebase'
    import {Card, CardBody, CardHeader, CardTitle, Col, Container, Input, Row, Spinner} from 'sveltestrap'
  	import Chat_Component from '../Misc/Chat.svelte'
    import type { Chat, Identity } from '@src/models/models'
  import { getIdentity } from '@src/services/backend/api'

    const initialisePage = async () => {
        let promises: Promise<Identity>[] = []
        $userStore.user.patients.forEach((patient) => {
            promises.push(getIdentity(patient).then((x) => x.obj))
      })
      return Promise.all(promises)
    }

    let pInit = initialisePage()
</script>

{#await pInit}
<Spinner/>
{:then participants}
<Card style="margin-top: 4rem;  background-color: lightblue; width: 40%">
    <CardHeader>
        <CardTitle>Dr. {$userStore.user.fullName}</CardTitle>
    </CardHeader>
    <CardBody style=" background-color: lightgrey;">
        <p>Number Of Patients: {participants.length}</p>
        
    </CardBody>
</Card>

<Chat_Component possibleParticipants={participants}/>
{/await}

