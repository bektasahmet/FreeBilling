// state
import { reactive } from "vue";
import http from "../http";

export default reactive({
    token: "",
    customers: [],
    employees: [],
    timebills: [],
    currentCustomer: null,
    async loadCustomers() {
        if (this.customers.length === 0) {
            const customerResult = await http.get("/api/customers");
            this.customers.splice(this.customers, this.customers.length, ...customerResult.data);
        } 
    },

    async loadEmployees() {
        if (this.employees.length === 0) {
            const employeeResult = await http.get("/api/employees");
            this.employees.splice(this.employees, this.employees.length, ...employeeResult.data);
        }
    },

    async loadTimeBills(customerId) {
        this.currentCustomerId = customerId;
        const result = await http.get(`/api/customers/${this.currentCustomerId}/timebills`);
        if (result.status === 200) {
            this.timebills.splice(0, this.timebills.length, ...result.data);
        }
    },

    async saveBill(bill) {
        const result = await http.post("/api/timebills", bill);
        if (result.status === 201) {
            if (result.data.customerId === this.currentCustomerId) {
                this.timebills.push(result.data);
            }
        }
    }
})