// router
import HomeView from "../views/HomeView.vue"
import BillingView from "../views/BillingView.vue"
import LoginView from "@/views/LoginView.vue"
import state from "@/state"
import { createRouter, createWebHashHistory, createWebHistory } from "vue-router";

const routes = [
    {
        name: "Home",
        path: "/",
        component: HomeView
    },
    {
        name: "Billing",
        path: "/billing",
        component: BillingView
    },
    {
        name: "Login",
        path: "/login",
        component: LoginView
    }
];

const router = createRouter({
    routes,
    history: createWebHistory()
});

router.beforeEach((to) => {
    if (to.name !== "Login") {
        if (!state.token) {
            return { name: "Login" }
        }
    }
});



export default router;