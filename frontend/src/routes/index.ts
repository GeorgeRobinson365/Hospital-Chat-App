import { Role } from '@src/models/models'
import Login from '@src/pages/Authentication/Login.svelte'
import DoctorHome from '@src/pages/Doctor/Index.svelte'
import Adminpanel from '@src/pages/Admin/Index.svelte'
import PatientHome from '@src/pages/Patient/Index.svelte'
import Confirmation__SvelteComponent_ from '@src/pages/Misc/Confirmation.svelte'

const authProtectedRoutes = [
    {path: '/doctor-home', component: DoctorHome,
    auth: {
        validRoles: [Role.Doctor]
    }},
    {path: '/patient-home', component: PatientHome,
    auth: {
        validRoles: [Role.Patient]
    }},
    { path:'/admin', component: Adminpanel,
        auth: { validRoles: [Role.Admin] 
    }}
]

const publicRoutes = [ 
    {path: '/', component:Login},
    {path: '/login', component:Login},
    {path: '/confirmation', component: Confirmation__SvelteComponent_}
]

const DATA = {
    authProtectedRoutes,
    publicRoutes
}

export default DATA