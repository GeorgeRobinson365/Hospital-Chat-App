<script lang="ts">
  import { Dropdown, DropdownItem, DropdownMenu, DropdownToggle } from "sveltestrap"
  import type {ComboBoxRenderer, ComboBox} from '@vaadin/combo-box'
  import type { Doctor } from "@src/models/models"

	 
	export let fullName;
	export let email;
	export let id;
	export let doctor;
	export let doctorsMap : Record<string, Doctor>;

	let comboBox: ComboBox<Doctor>
</script>

<div class='card'>
	
	<h2>{fullName}</h2>
	<p>{email}</p>
</div>

<style>
	.card {
		position: relative;
		margin: 0.5em;
		padding: 0.5em 0.5em 0.5em 6em;
		border: 1px solid #eee;
		border-radius: 4px;
		box-shadow: 2px 2px 4px rgba(0,0,0,0.1);
		min-height: 5em;
	}

	.card::after {
		clear: both;
		display: block;
	}

	

	h2 {
		margin: 0 0 0.5em 0;
		font-size: 16px;
	}

	p {
		margin: 0;
		font-size: 14px;
	}
</style>