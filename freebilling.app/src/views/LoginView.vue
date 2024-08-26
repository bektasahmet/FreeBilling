<script setup>
    import { ref } from "vue";
    import axios from "axios";
    import state from "@/state";
    import { useRouter } from "vue-router";

    const userName = ref("");
    const password = ref("");
    const message = ref("");

    const router = useRouter();

    async function login() {
        try {
            const result = await axios.post("/api/auth/token", {
                userName: userName.value,
                password: password.value
            });
            state.token = result.data.token;
            router.push("/");

        } catch {
            message.value = "Wrong username or password";
        }
    }

</script>

<template>
    <div class="w-96 mx-auto">
        <h3>Login</h3>
        <form novalidate @submit.prevent="login">
            <div v-if="message">{{ message }}</div>
            <label for="userName">Username</label>
            <input type="text" id="userName" v-model="userName" />

            <label for="password">Password</label>
            <input type="password" id="password" v-model="password"/>
            <button type="submit">Login</button>
        </form>
        

    </div>
</template>