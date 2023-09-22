<script lang="ts">
  import { logOut,exportAccount, deleteAccount } from '@src/helpers/usersManagement'
  import {Collapse, Nav, NavItem, NavLink, Navbar, NavbarBrand, NavbarToggler} from 'sveltestrap'
  import { userStore } from '@src/helpers/stores'

  let isOpen = false
  let userId = $userStore.user.id
  const toggle = () => (isOpen = !isOpen) 
  const handleDelete = () => {
    deleteAccount(userId);
  };
</script>

<Navbar color="primary" light style="text-align:center;">
    <NavbarToggler on:click={toggle} class="me-2" />
    <Collapse {isOpen} navbar>
      <Nav navbar>
        <NavItem>
          <button class="logOut" on:click={logOut}>Log out</button>
        </NavItem>
        <NavItem>
          <button class="export" on:click={exportAccount}>Export Account</button>
        </NavItem>
        <NavItem>
          <button class = "delete" on:click={handleDelete}>Delete Account</button>
        </NavItem>
      </Nav>
    </Collapse>
  </Navbar>

  <style>
    .logOut {
  background-color: transparent;
  border: none;
  color:white;
  font-size:3rem;
  }
  .export {
  background-color: transparent;
  border: none;
  color:white;
  font-size:3rem;
  }
  .delete {
  background-color: transparent;
  border: none;
  color:white;
  font-size:3rem;
  }
  </style>