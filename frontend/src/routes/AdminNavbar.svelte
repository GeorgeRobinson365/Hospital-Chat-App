<script lang="ts">
  import { logOut } from '@src/helpers/usersManagement'
    import {Collapse, Nav, NavItem, NavLink, Navbar, NavbarBrand, NavbarToggler} from 'sveltestrap'

    let isOpen = false
    const toggle = () => (isOpen = !isOpen) 
</script>
<div>
<Navbar color="primary" light style="text-align:center;">
    
    <NavbarToggler on:click={toggle} class="me-2" />
    <Collapse {isOpen} navbar>
      <Nav navbar>
        <NavItem>
          <button class="logOut" on:click={logOut}>Log out</button>
        </NavItem>
      </Nav>
    </Collapse>
    </Navbar>
</div>

<style>
  .logOut {
background-color: transparent;
border: none;
color:white;
font-size:3rem;
}
</style>