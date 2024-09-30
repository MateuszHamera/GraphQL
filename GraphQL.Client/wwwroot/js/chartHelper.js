window.renderPriceChart = (prices, times) => {
    const ctx = document.getElementById('bookPriceChart').getContext('2d');

    if (window.bookPriceChart) {
        window.bookPriceChart.data.labels = times;
        window.bookPriceChart.data.datasets[0].data = prices;
        window.bookPriceChart.update();
    } else {
        window.bookPriceChart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: times,
                datasets: [{
                    label: 'Book Price Over Time',
                    data: prices,
                    borderColor: 'rgba(75, 192, 192, 1)',
                    borderWidth: 2,
                    fill: false
                }]
            },
            options: {
                responsive: true,
                scales: {
                    x: {
                        title: {
                            display: true,
                            text: 'Time'
                        }
                    },
                    y: {
                        title: {
                            display: true,
                            text: 'Price'
                        },
                        beginAtZero: false
                    }
                }
            }
        });
    }
};
