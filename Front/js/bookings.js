// bookings.js - Handle booking operations

// Load user bookings
async function loadUserBookings() {
    try {
        const bookingsContainer = document.getElementById('user-bookings');
        if (!bookingsContainer) return;
        
        // Show loading indicator
        bookingsContainer.innerHTML = '<div class="text-center"><div class="spinner-border" role="status"><span class="visually-hidden">Loading...</span></div></div>';
        
        // Check if user is logged in
        if (!isLoggedIn()) {
            window.location.href = 'login.html';
            return;
        }
        
        // Fetch user bookings
        const bookings = await API.Bookings.getUserBookings();
        
        // Clear loading indicator
        bookingsContainer.innerHTML = '';
        
        if (bookings.length === 0) {
            bookingsContainer.innerHTML = '<div class="alert alert-info">You have no bookings yet.</div>';
            return;
        }
        
        // Create a table for bookings
        const table = document.createElement('table');
        table.className = 'table table-striped';
        
        table.innerHTML = `
            <thead>
                <tr>
                    <th>Event</th>
                    <th>Date</th>
                    <th>Tickets</th>
                    <th>Total Price</th>
                    <th>Status</th>
                </tr>
            </thead>
            <tbody id="bookings-table-body">
            </tbody>
        `;
        
        bookingsContainer.appendChild(table);
        
        const tableBody = document.getElementById('bookings-table-body');
        
        // Add each booking to the table
        bookings.forEach(booking => {
            const bookingDate = new Date(booking.bookingDate).toLocaleDateString('en-US');
            const status = booking.isConfirmed ? 'Confirmed' : 'Pending';
            
            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${booking.eventName}</td>
                <td>${bookingDate}</td>
                <td>${booking.ticketCount}</td>
                <td>$${booking.totalPrice.toFixed(2)}</td>
                <td><span class="badge bg-${booking.isConfirmed ? 'success' : 'warning'}">${status}</span></td>
            `;
            
            tableBody.appendChild(row);
        });
    } catch (error) {
        console.error('Error loading bookings:', error);
        document.getElementById('user-bookings').innerHTML = 
            `<div class="alert alert-danger">Error loading bookings: ${error.message}</div>`;
    }
}

// Initialize bookings if on the user bookings page
document.addEventListener('DOMContentLoaded', () => {
    const currentPage = window.location.pathname.split('/').pop();
    
    if (currentPage === 'user-bookings.html') {
        loadUserBookings();
    }
});