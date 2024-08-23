const formatter = Intl.NumberFormat("tr-TR", {
    style: "currency",
    currency: "TRY"
})

export function formatMoney(value) {
    if (typeof value === "number") {
        return formatter.format(value);
    }
    return value;
}