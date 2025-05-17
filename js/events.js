// events.js - Handle events display and operations

// Fetch and display all events
async function loadEvents() {
    try {
        const eventsContainer = document.getElementById('events-container');
        if (!eventsContainer) return;
        
        // Show loading indicator
        eventsContainer.innerHTML = '<div class="text-center"><div class="spinner-border" role="status"><span class="visually-hidden">Loading...</span></div></div>';
        
        // Fetch events from API
        const events = await API.Events.getAllEvents();
        
        // Clear loading indicator
        eventsContainer.innerHTML = '';
        
        if (events.length === 0) {
            eventsContainer.innerHTML = '<div class="alert alert-info">No events available at the moment.</div>';
            return;
        }
        
        // Create a row for grid layout
        const row = document.createElement('div');
        row.className = 'row row-cols-1 row-cols-md-3 g-4';
        
        // Render each event as a card
        events.forEach(event => {
            const card = createEventCard(event);
            row.appendChild(card);
        });
        
        eventsContainer.appendChild(row);
    } catch (error) {
        console.error('Error loading events:', error);
        document.getElementById('events-container').innerHTML = 
            `<div class="alert alert-danger">Error loading events: ${error.message}</div>`;
    }
}

// Create an event card element
function createEventCard(event) {
    const col = document.createElement('div');
    col.className = 'col';
    
    const formattedDate = new Date(event.date).toLocaleDateString('en-US', {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    });
    
    const bookButtonHtml = event.isBooked 
        ? '<button class="btn btn-secondary" disabled>Booked</button>'
        : `<a href="event-details.html?id=${event.id}" class="btn btn-primary">Book Now</a>`;
    
    col.innerHTML = `
        <div class="card h-100">
            <img src="${event.imageUrl || 'img/event-placeholder.jpg'}" class="card-img-top" alt="${event.name}">
            <div class="card-body">
                <h5 class="card-title">${event.name}</h5>
                <p class="card-text">${event.description.substring(0, 100)}${event.description.length > 100 ? '...' : ''}</p>
            </div>
            <ul class="list-group list-group-flush">
                <li class="list-group-item"><i class="bi bi-calendar"></i> ${formattedDate}</li>
                <li class="list-group-item"><i class="bi bi-geo-alt"></i> ${event.venue}</li>
                <li class="list-group-item"><i class="bi bi-tag"></i> ${event.category}</li>
                <li class="list-group-item"><i class="bi bi-currency-dollar"></i> ${event.price.toFixed(2)}</li>
            </ul>
            <div class="card-footer">
                ${bookButtonHtml}
            </div>
        </div>
    `;
    
    return col;
}

// Load single event details by ID
async function loadEventDetails(eventId) {
    try {
        const eventDetailsContainer = document.getElementById('event-details-container');
        if (!eventDetailsContainer) return;
        
        // Show loading indicator
        eventDetailsContainer.innerHTML = '<div class="text-center"><div class="spinner-border" role="status"><span class="visually-hidden">Loading...</span></div></div>';
        
        // Fetch event details from API
        const event = await API.Events.getEventById(eventId);
        
        const formattedDate = new Date(event.date).toLocaleDateString('en-US', {
            year: 'numeric',
            month: 'long',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });
        
        // Display event details
        eventDetailsContainer.innerHTML = `
            <div class="card mb-3">
                <div class="row g-0">
                    <div class="col-md-4">
                        <img src="${event.imageUrl || 'img/event-placeholder.jpg'}" class="img-fluid rounded-start" alt="${event.name}">
                    </div>
                    <div class="col-md-8">
                        <div class="card-body">
                            <h2 class="card-title">${event.name}</h2>
                            <p class="card-text">${event.description}</p>
                            <ul class="list-group list-group-flush mb-3">
                                <li class="list-group-item"><strong>Category:</strong> ${event.category}</li>
                                <li class="list-group-item"><strong>Date:</strong> ${formattedDate}</li>
                                <li class="list-group-item"><strong>Venue:</strong> ${event.venue}</li>
                                <li class="list-group-item"><strong>Price:</strong> $${event.price.toFixed(2)}</li>
                            </ul>
                            ${event.isBooked 
                                ? '<button class="btn btn-lg btn-secondary" disabled>Already Booked</button>'
                                : '<button id="book-btn" class="btn btn-lg btn-primary">Book Now</button>'
                            }
                        </div>
                    </div>
                </div>
            </div>
        `;
        
        // Add event listener to book button if not booked
        if (!event.isBooked) {
            document.getElementById('book-btn').addEventListener('click', () => bookEvent(event.id));
        }
    } catch (error) {
        console.error('Error loading event details:', error);
        document.getElementById('event-details-container').innerHTML = 
            `<div class="alert alert-danger">Error loading event details: ${error.message}</div>`;
    }
}

// Book an event
async function bookEvent(eventId) {
    try {
        // Check if user is logged in
        if (!isLoggedIn()) {
            window.location.href = 'login.html';
            return;
        }
        
        const bookBtn = document.getElementById('book-btn');
        bookBtn.disabled = true;
        bookBtn.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Booking...';
        
        // Create booking with 1 ticket (as per requirements)
        const bookingData = {
            eventId: eventId,
            ticketCount: 1
        };
        
        await API.Bookings.createBooking(bookingData);
        
        // Show congratulations message
        document.getElementById('event-details-container').innerHTML = `
            <div class="alert alert-success text-center">
                <h2><i class="bi bi-check-circle-fill"></i> Congratulations!</h2>
                <p class="lead">Your booking has been confirmed.</p>
                <a href="index.html" class="btn btn-primary mt-3">Back to Events</a>
            </div>
        `;
    } catch (error) {
        console.error('Error booking event:', error);
        
        // Re-enable button and show error
        const bookBtn = document.getElementById('book-btn');
        if (bookBtn) {
            bookBtn.disabled = false;
            bookBtn.textContent = 'Book Now';
        }
        
        alert(`Error booking event: ${error.message}`);
    }
}

// Initialize events based on current page
document.addEventListener('DOMContentLoaded', () => {
    // Get current page
    const currentPage = window.location.pathname.split('/').pop();
    
    // Load events for different pages
    if (currentPage === 'index.html' || currentPage === '') {
        loadEvents();
    } else if (currentPage === 'event-details.html') {
        // Get event ID from URL
        const urlParams = new URLSearchParams(window.location.search);
        const eventId = urlParams.get('id');
        
        if (eventId) {
            loadEventDetails(eventId);
        } else {
            document.getElementById('event-details-container').innerHTML = 
                '<div class="alert alert-danger">No event ID specified</div>';
        }
    }
});