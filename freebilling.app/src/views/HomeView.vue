<script setup>
    import { ref, reactive, computed, onMounted, watch} from "vue";
    import { formatMoney } from "@/formatters";
    import axios from "axios";
    import WaitCursor from "@/components/WaitCursor.vue"
    import state from "@/state"

    const name = ref("Ahmet");
    const john = ref("John");
    const isBusy = ref(false);
    const customerId = ref(0);

  

    function changeMe(){
        name.value += " +";
    }


    function newItem() {
        state.timebills.push({
            customerId: 4,
            employeeId: 4,
            hoursWorked: 5.0,
            rate: 114,
            work: "More work",
            date: "2023-05-08"
        });
    }

    const total = computed(() => {
        return state.timebills.map(b => b.billingRate * b.hours)
            .reduce((b, t) => t + b, 0);
    });

    onMounted(async () => {
        try {
            isBusy.value = true;
            await state.loadCustomers();
            
        } catch {
            console.log("Failed");
        } finally {
            setTimeout(() => isBusy.value = false, 1000);
        }

    });

    watch(customerId, async() => {
        try {
            isBusy.value = true;
            await state.loadTimeBills(customerId.value);
        } catch {
            isBusy.value = false;
        }
    })
</script>

<template>
    <header class="flex text-red-900">
        <h1 class="text-red-900">Our App</h1>
    </header>

    <main>
        <h1>Hello from Vue</h1>
        <WaitCursor :busy="isBusy" msg="Please Wait..."></WaitCursor>
        <!--<div> {{ name }}</div>
        <button class="btn" @click="changeMe">Click Me!</button>
        <img src="/john.jpg" v-bind:alt="john" v:bindtitle="john.toUpperCase()"/>
        <button class="btn" @click="newItem">New Item</button>-->
        <div>
            <label>Customers</label>
            <select class="w-96 mx-2" v-model="customerId">
                <option v-for="c in state.customers" :value="c.id">{{ c.companyName }}</option>
            </select>
        </div>
        <table class="w-full">
            <thead>
                <tr class="text-bold bg-blue-800 text-white">
                    <td>Hours</td>
                    <td>Date</td>
                    <td>Description</td>
                    <td>Rate</td>
                    <td>Employee</td>
                </tr>
            </thead>

            <tbody>
                <tr v-for="b in state.timebills">
                    <td>{{ b.hours }}</td>
                    <td>{{ b.date }}</td>
                    <td>{{ b.workPerformed }}</td>
                    <td>{{ b.billingRate }}</td>
                    <td>{{ b.employee.name }}</td>

                </tr>
            </tbody>
        </table>
        <div>Total: {{ formatMoney(total) }}</div>

    </main>
</template>
