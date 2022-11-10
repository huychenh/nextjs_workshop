const formatter = new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
});

export function formatCurrency(number: number) {
    return formatter.format(number);
}
