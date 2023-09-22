<script lang="ts">
  import type { SendMessageCommand } from '@src/models/models'
  import { userStore } from './../../helpers/stores'
  import ChatMessage from '../Misc/ChatMessage.svelte'
  // @ts-ignore
  import Fa from 'svelte-fa/src/fa.svelte'
  // @ts-ignore
  import {
    faUsers,
    faCompressArrowsAlt,
    faComments,
    faEnvelope,
    faWindowMinimize,
  } from '@fortawesome/free-solid-svg-icons'
  import { ComboBox } from '@vaadin/combo-box'
  import ChatContact from '../Misc/ChatContact.svelte'
  import { each } from 'svelte/internal'
  import {
    type Chat,
    type Identity,
    Role,
    Doctor,
    Message,
    CreateChatCommand,
  } from '@src/models/models'
  import {
    getPatients,
    getDoctors,
    getIdentity,
    getChats,
    getDoctor,
    getMessages,
    createChat,
    sendMessage,
  } from '@src/services/backend/api'
  import { ECDH } from 'crypto'
  import { Input, Spinner } from 'sveltestrap'
  import { Writable } from 'svelte/store'
  import log from 'loglevel'


  export let isDoctor: boolean = false
  export let possibleParticipants: Identity[] | Doctor[] = []
  type Contact = {
    id: string
    name: string
    lastMessage: string
    lastMessageTimestamp: number
    unreadMessages: boolean
  }

  let chats: Chat[]
  let messages: Message[] = []
  let contacts: Contact[] = []

  let currentMessages = []
  $: currentMessages = currentMessages

  const initialiseChat = async (): Promise<[Chat[], Message[], Identity[] | Doctor[]]> => {
    chats = (await getChats($userStore.user.id)).obj
    messages = (await getMessages($userStore.user.id)).obj
    convertParticipantsToContacts()
    return [chats, messages, possibleParticipants]
  }

  let pInit: Promise<[Chat[], Message[], Identity[] | Doctor[]]> = initialiseChat()

  let convertParticipantToContact = (participant: Chat): Contact => {
    let lastMessage = messages
      .filter((x) => x.chatId == participant.chatId)
      ?.sort((a, b) => new Date(b.sentAt).getTime() - new Date(a.sentAt).getTime())[0]
    return {
      id: participant.chatId,
      name: participant.name,
      lastMessage: lastMessage?.content,
      lastMessageTimestamp: new Date(lastMessage?.sentAt).getTime(),
      unreadMessages: false,
    }
  }

  let convertParticipantsToContacts = () => {
	contacts = []
    chats.forEach((x) => {
      contacts.push(convertParticipantToContact(x))
    })
  }

  let isOpen = true
  let currentName: string = ''

  let messageInput = ''

  const openContacts = () => {
    isOpen = !isOpen
  }
  function openChat(contact: Contact) {
    currentName = contact.name
    currentMessages = messages.filter((x) => x.chatId.includes(contact.id))
    isOpen = false
    // do something with the clicked contact
  }

  const onParticipantChanged = (e: CustomEvent<any>) => {
    selectedParticipant = possibleParticipants.find((x) => x.id == e.detail.value)
  }

  const onCreateChat = async () => {
    let createChatCmd: CreateChatCommand = {
      participantId: [$userStore.user.id, selectedParticipant.id],
    }
    let chat = (await createChat(createChatCmd)).obj
    let sendMessageCommand: SendMessageCommand = {
      chatId: chat.id,
      content: messageText,
      senderId: $userStore.user.id,
    }
    let msg = (await sendMessage(sendMessageCommand)).obj
    chats.push(chat)
    contacts.push(convertParticipantToContact(chat))
  }

  const onSendMessage = async (input: string) => {
    let sendMessageCommand: SendMessageCommand = {
      chatId: currentMessages[0].chatId,
      content: input,
      senderId: $userStore.user.id,
    }
    let msg = (await sendMessage(sendMessageCommand)).obj
    currentMessages = [...currentMessages, msg]
	messages.push(msg)
	console.log(chats, possibleParticipants)
	console.log("sasa")
	log.info(chats)
	log.info(possibleParticipants)
	possibleParticipants = possibleParticipants.filter((x) => chats.find((y) => y.chatId.includes(x.id)) == undefined)
	messageInput = ""
	convertParticipantsToContacts()
	return
  }

  let selectedParticipant: Identity | Doctor
  let isCreateChatOpen = false
  let messageText: string = ''
</script>

{#await pInit}
<Spinner />
{:then}
  {#if isCreateChatOpen}
    <vaadin-combo-box
      renderer={(root, comboBox, model) => {
        const item = model.item
        root.innerHTML = `<b>${item.label}</b>`
      }}
      items={possibleParticipants.map((doctor) => {
        return { label: doctor.fullName, value: doctor.id }
      })}
      on:value-changed={onParticipantChanged}
      item-value-path="value"
      item-label-path="label" />
    <Input bind:value={messageText} />
    <button on:click={() => onCreateChat()}>Create Chat</button>
  {:else}
    <button on:click={() => (isCreateChatOpen = !isCreateChatOpen)}>Create Chat</button>
  {/if}

  {#if isOpen}
    <div class="card card-danger direct-chat direct-chat-danger">
      <div class="card-header">
        <div class="card-tools d-flex">
          <span>Chats List</span>
          <span class="mr-auto" />
          <button on:click={() => openContacts()} type="button" class="btn btn-tool"
            ><Fa icon={faWindowMinimize} /></button>
        </div>
      </div>
      <div class="card-body">
        <div class="direct-chat-contacts">
          <ul class="contacts-list">
            {#each contacts as contact}
              <li on:click={() => openChat(contact)}>
                <ChatContact
                  name={contact.name}
                  lastMessage={contact.lastMessage}
                  lastMessageTimestamp={contact.lastMessageTimestamp}
                  unreadMessages={contact.unreadMessages} />
              </li>
            {/each}
          </ul>
        </div>
      </div>
    </div>
  {:else}
    {#key currentMessages}
      <div class="card card-danger direct-chat direct-chat-danger">
        <div class="card-header">
          <div class="card-tools d-flex">
            <span class="contacts-name">{currentName}</span>
            <span class="mr-auto" />
            <button
              on:click={() => openContacts()}
              type="button"
              class="btn btn-tool"
              title="Contacts"><Fa icon={faUsers} /></button>
            <button type="button" class="btn btn-tool"><Fa icon={faCompressArrowsAlt} /></button>
          </div>
        </div>
        <div class="card-body">
          <div class="direct-chat-messages">
            {#each currentMessages as message}
              <ChatMessage
                message={message.content}
                timestamp={message.sentAt}
                sentByMe={message.senderId == $userStore.user.id} />
            {/each}
          </div>
        </div>
        <div class="card-footer">
          <div class="input-group">
            <input
              type="text"
              bind:value={messageInput}
              placeholder="Type Message ..."
              class="form-control" />
            <span class="input-group-append">
              <button
                on:click={() => onSendMessage(messageInput)}
                type="button"
                class="btn btn-primary"
                style="width: 60px;">Send</button>
            </span>
          </div>
        </div>
      </div>
    {/key}
  {/if}
{/await}

<style>
  .direct-chat-contacts {
    transition: -webkit-transform 0.5s ease-in-out;
    transition: transform 0.5s ease-in-out;
    transition: transform 0.5s ease-in-out, -webkit-transform 0.5s ease-in-out;
    -webkit-transform: translate(0, 0);
    transform: translate(0, 0);
    height: 250px;
    overflow: auto;
    width: 100%;
  }

  .contacts-list {
    padding-left: 0;
    list-style: none;
  }

  li {
    border-bottom: 1px solid rgba(0, 0, 0, 0.2);
    margin: 0;
    padding: 10px;
    cursor: pointer;
  }

  li::after {
    display: block;
    clear: both;
    content: '';
  }

  li:last-of-type {
    border-bottom: 0;
  }

  .direct-chat .card-body {
    overflow-x: hidden;
    padding: 0;
    position: relative;
  }

  .direct-chat-messages {
    -webkit-transform: translate(0, 0);
    transform: translate(0, 0);
    height: 400px;
    overflow: auto;
    padding: 10px;
    transition: -webkit-transform 0.5s ease-in-out;
    transition: transform 0.5s ease-in-out;
    transition: transform 0.5s ease-in-out, -webkit-transform 0.5s ease-in-out;
  }
  .contacts-name {
    margin-left: 15px;
    font-weight: 600;
  }
</style>
