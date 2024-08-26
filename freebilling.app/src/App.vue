<script setup>
    import state from "@/state";
    import { useRouter } from "vue-router";
    import router from "./router/index";

    function logout() {
        state.token = "";
        router.push("/");
    }
</script>

<template>
    <ul class="text-blue-900 font-bold flex justify-end">
        <li class="mr-2"><RouterLink to="/">Home</RouterLink></li>
        <li class="mr-2"><RouterLink to="/Billing">Billing</RouterLink></li>
        <li class="mr-2"><RouterLink v-if="!state.token" :to="{ name: 'Login' }">Login</RouterLink></li>
        <li><a href="" v-if="state.token" @click="logout">Logout</a></li>
    </ul> 

    <div class="bg-gray-300 p-2">
        <RouterView></RouterView>
    </div>
</template>