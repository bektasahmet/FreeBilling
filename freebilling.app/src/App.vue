<script setup>
    import { ref } from "vue";
    const name = ref("Ahmet");
    const john = ref("John");

    const bills = [
        {
          "hoursWorked": 2.5,
          "rate": 125.00,
          "date": "2024-08-19",
          "work": "Added 1.0",
          "customerId": 1,
          "employeeId": 1
        },

        {
          "hoursWorked": 2.0,
          "rate": 126.00,
          "date": "2024-08-20",
          "work": "Added 1.2",
          "customerId": 2,
          "employeeId": 2
        },

        {
          "hoursWorked": 2.5,
          "rate": 127.00,
          "date": "2024-08-21",
          "work": "Added 1.3",
          "customerId": 3,
          "employeeId": 3
        }
    
    ];

    function changeMe(){
        name.value += " +";
        
    }
</script>

<template>
  <header class="flex text-red-900">
    <h1 class="text-red-900">Our App</h1>
  </header>

  <main>
    <h1>Hello from Vue</h1>
    <div> {{ name }}</div>
    <button class="btn" @click="changeMe">Click Me!</button>
    <img src="/public/john.jpg" v-bind:alt="john" v:bindtitle="john.toUpperCase()"/>
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
                <td>{{ b.hoursWorked }}</td>
                <td>{{ b.date }}</td>
                <td>{{ b.work }}</td>
            </tr>
        </tbody>
    </table>

  </main>
</template>
