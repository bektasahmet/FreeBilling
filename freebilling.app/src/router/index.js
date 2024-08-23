// router
import HomeView from "../views/HomeView.vue"
import BillingView from "../views/BillingView.vue"
import LoginView from "@/views/LoginView.vue"
import state from "@/state"
import { createRouter, createWebHashHistory, createWebHistory } from "vue-router";

const routes = [
    {
    path: "/",
    component: HomeView
    },
    {
        path: "/billing",
        component: BillingView
    },
    {
        path: "/Login",
        component: LoginView
    }
];

const router = createRouter({
    routes,
    history: createWebHistory()
});



export default router;