<script lang="ts">
	import { ComboBox } from '@vaadin/combo-box';
  import { auth } from '@src/helpers/firebase'
  import { Card, CardBody, CardHeader, CardTitle, Col, Container, Row } from 'sveltestrap'
  import { onMount } from 'svelte'
  import { getDoctors, getPatients, getPendingIdentities, submitApproval, submitPatientAccess } from '@src/services/backend/api'
  import { CreateIdentityCommand, Doctor, Identity, Patient, Role, SubmitPatientAccessComand } from '@src/models/models'
  const patientendpoint = '?'
  const doctorendpoint = '?'
  let unauthorisedpatients: Identity[] = []
  let unauthoriseddoctors: Identity[] = []

  import VirtualList from './VirtualList.svelte';
	
	import ListItem from './ListItem.svelte';

  let patients : Patient[] = []
	let searchTerm = ""; // define your searchTerm
	// $: filteredList2 = combined.filter(item => item.name.indexOf(filteredList) !== -1);
  
  let comboBox: ComboBox

  let start;
  let end;
	let filteredList : Patient[] = []
  let doctors: Doctor[] = []
  let doctorsMap : Record<string, Doctor> = {}
  onMount(async function () {
    const res = await getPendingIdentities()
    doctors = (await getDoctors()).obj
    doctors.forEach((doctor) => {
      doctor.patients = doctor.patients == null ? [] : doctor.patients
      doctorsMap[doctor.id] = doctor
    })
    patients = (await getPatients()).obj
    patients.forEach((patient) => {
      patient.doctor = doctors.find((doctor) => doctor.patients.includes(patient.id))?.id
    })
    const identities = res.obj
    unauthorisedpatients = identities.filter((identity) => identity.role == Role.PendingPatient)
    unauthoriseddoctors = identities.filter((identity) => identity.role == Role.PendingDoctor)
  })
  $: filteredList = patients.filter(item => item.fullName?.toLowerCase().includes(searchTerm.toLowerCase()));

  const onAcceptButtonClicked = (identity: Identity) : Promise<void> => {
    return submitApproval(identity.id).then(() => {
      unauthoriseddoctors = unauthoriseddoctors.filter((doctor) => doctor.id != identity.id)
    })
  }

  const onDoctorChanged = async (e: CustomEvent, patient: Patient) => {
    console.log(e)
    let cmd: SubmitPatientAccessComand = {
      PatientId: patient.id,
      DoctorId: e.detail.value
    }
    await submitPatientAccess(cmd)
  }
</script>

Filter: <input bind:value={searchTerm} />
{searchTerm}

<div class='container'>
	<VirtualList items={filteredList} bind:start bind:end let:item>
		<ListItem {...item} {doctorsMap}/>
    <vaadin-combo-box 
    id="combo-box-${item.id}" 
    value={doctorsMap[item.doctor]?.id ?? "Select Doctor"}
    on:value-changed={(e)=>onDoctorChanged(e, item)}
    renderer={(root, comboBox, model) => {
      const item = model.item;
       root.innerHTML = `<b>${item.label}</b>`; }}
    items={doctors.map((doctor) => {return {label: doctor.fullName, value: doctor.id}})}
    bind:this={comboBox} >
    </vaadin-combo-box>

	</VirtualList>
	<p>showing items {start}-{end}</p>
</div>



<Card>
  <h1>Unauthorised patients: <br /></h1>
  {#each unauthorisedpatients as article}
    <div class="box">
    <p>{article.fullName}</p>
    <button on:click={()=>onAcceptButtonClicked(article)}>Accept</button>
  </div>
  {:else}
    <p>No unauthorised patients</p>
  {/each}
  <h1>Unauthorised doctors: <br /></h1>
  {#each unauthoriseddoctors as article}
    <div class="box">
      <p>{article.fullName}</p>
      <button on:click={()=>onAcceptButtonClicked(article)}>Accept</button>
    </div>
  {:else}
    <p>No unauthorised doctors</p>
  {/each}
</Card>

<style>
  .box {
    background-color: #f5f5f5;
    border: 1px solid #e3e3e3;
    border-radius: 5px;
    padding: 10px;
    margin: 10px;
    display: inline-flex;
  }
  .container {
		border-top: 1px solid #333;
		border-bottom: 1px solid #333;
		min-height: 200px;
		height: calc(100vh - 15em);
	}
</style>
