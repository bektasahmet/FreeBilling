//http.js
import state from "@/state";
import axios from "axios";

export default {
    async get(url) {
        return await axios.get(url, {
            headers: {
                authorization: `Bearer ${state.token}`
            }
        });
    },
    async post(url, data) {
        return await axios.post(url, data, {
            headers: {
                authorization: `Bearer ${state.token}`
            }
        });
    }
}