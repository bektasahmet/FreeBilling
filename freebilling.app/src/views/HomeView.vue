<script setup>
    import { ref, reactive, computed, onMounted } from "vue";
    import { formatMoney } from "@/formatters";
    import axios from "axios";
    import WaitCursor from "@/components/WaitCursor.vue"

    const name = ref("Ahmet");
    const john = ref("John");
    const isBusy = ref(false);

    const bills = reactive([
        //{
        //  "hoursWorked": 2.5,
        //  "rate": 125.00,
        //  "date": "2024-08-19",
        //  "work": "Added 1.0",
        //  "customerId": 1,
        //  "employeeId": 1
        //},

        //{
        //  "hoursWorked": 2.0,
        //  "rate": 126.00,
        //  "date": "2024-08-20",
        //  "work": "Added 1.2",
        //  "customerId": 2,
        //  "employeeId": 2
        //},

        //{
        //  "hoursWorked": 2.5,
        //  "rate": 127.00,
        //  "date": "2024-08-21",
        //  "work": "Added 1.3",
        //  "customerId": 3,
        //  "employeeId": 3
        //}

    ]);

    function changeMe(){
        name.value += " +";
    }


    function newItem() {
        bills.push({
            customerId: 4,
            employeeId: 4,
            hoursWorked: 5.0,
            rate: 114,
            work: "More work",
            date: "2023-05-08"
        });
    }

    const total = computed(() => {
        return bills.map(b => b.billingRate * b.hours)
            .reduce((b, t) => t + b, 0);
    });

    onMounted(async () => {
        try {
            isBusy.value = true;
            const result = await axios("/api/customers/1/timebills");
            if (result.status === 200) {
                bills.splice(0, bills.length, ...result.data);
            }
        } catch {
            console.log("Failed");
        } finally {
            setTimeout(() => isBusy.value = false, 1000);
        }

    });
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
        <table>
            <thead>
                <tr>
                    <td>Hours</td>
                    <td>Date</td>
                    <td>Description</td>
                </tr>
            </thead>

            <tbody>
                <tr v-for="b in bills">
                    <td>{{ b.hours }}</td>
                    <td>{{ b.date }}</td>
                    <td>{{ b.workPerformed }}</td>
                </tr>
            </tbody>
        </table>
        <div>Total: {{ formatMoney(total) }}</div>

    </main>
</template>
