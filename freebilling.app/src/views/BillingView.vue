<script setup>
    import { onMounted, ref, reactive } from "vue";

    import state from "@/state";
    import { useRouter } from "vue-router"

    const router = useRouter();
    const bill = ref({});
    const employees = reactive([]);
    const customers = reactive([]);
    const message = ref("");

    onMounted(async () => {
        try {

            await state.loadEmployees();

            await state.loadCustomers();

        } catch (e) {
            message.value = e;
        }

    });

    async function saveBill() {
        try {
            await state.saveBill(bill.value);
            router.push("/");
        } catch(e){
            message.value = e;
        }
    }
</script>



<template>
    <div class="w-96 mx-auto bg-white p-2">
        <h1>Billing</h1>
        <div v-if="message">{{ message }}</div>
        <form novalidate @submit.prevent="saveBill">

            <label for="time">Date</label>
            <input type="date" name="date" id="date" v-model="bill.date" />

            <label for="time">Hours Worked</label>
            <input type="text" name="time" id="time" v-model="bill.hoursWorked" />

            <label for="workPerformed">Work Performed</label>
            <textarea rows="4" name="workPerformed" id="workPerformed" v-model="bill.work"></textarea>

            <label for="employee">Employee</label>
            <select id="employee" name="employee" v-model="bill.employeeId">
                <option v-for="e in state.employees" :key="e.id" :value="e.id">{{ e.name }}</option>
            </select>

            <label for="rate">Rate</label>
            <input type="number" id="rate" v-model="bill.rate" />

            <label for="client">Client</label>
            <select id="client" name="client" v-model="bill.customerId">
                <option v-for="c in state.customers" :value="c.id" :key="c.id">{{ c.companyName }}</option>
            </select>

            <div class="mt-2">
                <button type="submit" value="Save" class="bg-green-800 hover:bg-green-700 mr-2">Save</button>
                <button type="submit" value="Cancel">Cancel</button>
            </div>
        </form>
        <pre>{{ bill }}</pre>
    </div>
    

</template>