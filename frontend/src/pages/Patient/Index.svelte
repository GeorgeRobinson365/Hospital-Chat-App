<script lang="ts">
    import { userStore } from '@src/helpers/stores'
  import { Card, Spinner } from "sveltestrap"
  import Chat from '@src/pages/Misc/Chat.svelte'
  import { getDoctor } from '@src/services/backend/api'

  const defaultDoctor = {
    id: "-1",
    fullName: "No doctor assigned",
    patients: [] 
};

  const initialisePage = () => {
    return getDoctor($userStore.user.id).then(x=>x.obj)
    .catch(err=>{
      console.log(err)
      return defaultDoctor;
    })
  }

  let pInit = initialisePage()
</script>


{#await pInit}

<Spinner/>
{:then possibleParticipants} 
  
<Card>
    Patient Name: {$userStore.user.fullName} <br>
    Doctor Name: {possibleParticipants.fullName}
</Card>
<Chat isDoctor={false} possibleParticipants={[possibleParticipants]}/>

{/await}
