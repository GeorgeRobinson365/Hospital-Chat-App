<script>
	import Fa from 'svelte-fa';
	import { faCheck, faCheckDouble } from '@fortawesome/free-solid-svg-icons';
	
    export let sentByMe;
	export let message;
    export let timestamp;		
</script>

<style>
.direct-chat-msg,
.direct-chat-text {
	display: block;
}
.direct-chat-msg {
	margin-bottom: 10px;
}
.direct-chat-msg:before,
.direct-chat-msg:after {
	content: " ";
	display: table;
}
.direct-chat-msg:after {
	clear: both;
}
.direct-chat-text {
	border-radius: 5px;
	position: relative;
	padding: 5px 10px;
	border: 1px solid #d2d6de;
	margin: 2px 0 5px 50px;
	color: #444;
			margin-right: 50px;
}

.direct-chat-text.me {
	border-radius: 5px;
	position: relative;
	padding: 5px 10px;
	border: 1px solid #d2d6de;
	background-color: blue;
	margin: 2px 0 5px 50px;
	color: white;
			margin-right: 50px;
}
.direct-chat-text:after,
.direct-chat-text:before {
	position: absolute;
	right: 100%;
	top: 15px;
	border: solid transparent;
	border-right-color: #d2d6de;
	content: ' ';
	height: 0;
	width: 0;
	pointer-events: none;
}
.direct-chat-text:after {
	border-width: 5px;
	margin-top: -5px;
}
.direct-chat-text:before {
	border-width: 6px;
	margin-top: -6px;
}
.right .direct-chat-text {
	margin-right: 50px;
	margin-left: 50px;
}
.right .direct-chat-text:after,
.right .direct-chat-text:before {
	right: auto;
	left: 100%;
	border-right-color: transparent;
	border-left-color: #d2d6de;
}
</style>

<div class="direct-chat-msg" class:right="{sentByMe}" class:left="{!sentByMe}">
    <div class="direct-chat-infos clearfix">


        <span class="direct-chat-timestamp" class:float-left="{sentByMe}" class:float-right="{!sentByMe}">
					
			{new Date(timestamp).toLocaleString([], {year: 'numeric', month: '2-digit', day: '2-digit', hour: '2-digit', minute:'2-digit', hour12: false})}
					
					
			</span>
    </div>
    <div class="direct-chat-text {sentByMe ? "me" : ""}">
		<div class="d-flex">
			<span class="mr-auto">{message}</span>

		</div>
	</div>
</div>