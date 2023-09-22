<script>
    export let name;
export let lastMessage;
export let lastMessageTimestamp;
    export let unreadMessages = false;
    import { faCheck, faCheckDouble, faCircle } from '@fortawesome/free-solid-svg-icons';
    import Fa from 'svelte-fa'
</script>

<style>
.contacts-list-info {
    color: #000000;
    margin-left: 45px;
}

.contacts-list-name {
    display: block;
    font-weight: 600;
}

.contacts-list-date {
    color: #ced4da;
    font-weight: normal;
}

</style>
<div class="contacts-list-info">
    <span class="contacts-list-name">
        {name}
        {#if lastMessageTimestamp > 0}			
                <small class="contacts-list-date float-right">{new Date(lastMessageTimestamp).toLocaleDateString()}</small>

        {/if}
    
</div>



