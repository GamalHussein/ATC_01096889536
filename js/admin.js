// admin.js - Handle admin panel operations

// Load all events for admin
async function loadEventsForAdmin() {
    try {
        const eventsTableBody = document.getElementById('events-table-body');
        if (!eventsTableBody) return;
        
        // Show loading indicator
        eventsTableBody.innerHTML = '<tr><td colspan="6" class="text-center"><div class="spinner-border" role="status"><span class="visually-hidden">Loading...</span></div></td></tr>';
        
        // Ensure user is admin
        if (!requireAdmin()) return;
        
        // Fetch all events
        const events = await API.Events.getAllEvents();
        
        // Clear loading indicator
        eventsTableBody.innerHTML = '';
        
        if (events.length === 0) {
            eventsTableBody.innerHTML = '<tr><td colspan="6" class="text-center">No events available</td></tr>';
            return;
        }
        
        // Add each event to the table
        events.forEach(event => {
            const eventDate = new Date(event.date).toLocaleDateString('en-US');
            
            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${event.id}</td>
                <td>${event.name}</td>
                <td>${event.category}</td>
                <td>${eventDate}</td>
                <td>$${event.price.toFixed(2)}</td>
                <td>
                    <button class="btn btn-sm btn-primary edit-event" data-event-id="${event.id}">Edit</button>
                    <button class="btn btn-sm btn-danger delete-event" data-event-id="${event.id}">Delete</button>
                </td>
            `;
            
            eventsTableBody.appendChild(row);
        });
        
        // Add event listeners to buttons
        document.querySelectorAll('.edit-event').forEach(button => {
            button.addEventListener('click', () => editEvent(button.getAttribute('data-event-id')));
        });
        
        document.querySelectorAll('.delete-event').forEach(button => {
            button.addEventListener('click', () => deleteEvent(button.getAttribute('data-event-id')));
        });
    } catch (error) {
        console.error('Error loading events for admin:', error);
        document.getElementById('events-table-body').innerHTML = 
            `<tr><td colspan="6" class="text-center text-danger">Error loading events: ${error.message}</td></tr>`;
    }
}

// Load event data into form for editing
async function editEvent(eventId) {
    try {
        // Fetch event details
        const event = await API.Events.getEventById(eventId);
        
        // Populate form fields
        document.getElementById('event-form-title').textContent = 'Edit Event';
        document.getElementById('event-id').value = event.id;
        document.getElementById('event-name').value = event.name;
        document.getElementById('event-description').value = event.description;
        document.getElementById('event-category').value = event.category;
        
        // Format date for date input field (YYYY-MM-DDThh:mm)
        const dateObj = new Date(event.date);
        const formattedDate = dateObj.toISOString().slice(0, 16);
        document.getElementById('event-date').value = formattedDate;
        
        document.getElementById('event-venue').value = event.venue;
        document.getElementById('event-price').value = event.price;
        document.getElementById('event-image-url').value = event.imageUrl || '';
        
        // Show the form
        document.getElementById('event-form-container').classList.remove('d-none');
        document.getElementById('event-form-container').scrollIntoView({ behavior: 'smooth' });
    } catch (error) {
        console.error('Error loading event for editing:', error);
        alert(`Error loading event: ${error.message}`);
    }
}

// Delete an event
async function deleteEvent(eventId) {
    try {
        if (!confirm('Are you sure you want to delete this event? This action cannot be undone.')) {
            return;
        }
        
        // Delete the event
        await API.Events.deleteEvent(eventId);
        
        // Reload events table
        loadEventsForAdmin();
        
        // Show success message
        alert('Event deleted successfully');
    } catch (error) {
        console.error('Error deleting event:', error);
        alert(`Error deleting event: ${error.message}`);
    }
}

// Show form for creating a new event
function showCreateEventForm() {
    // Reset form
    document.getElementById('event-form').reset();
    document.getElementById('event-form-title').textContent = 'Create New Event';
    document.getElementById('event-id').value = '';
    
    // Show the form
    document.getElementById('event-form-container').classList.remove('d-none');
    document.getElementById('event-form-container').scrollIntoView({ behavior: 'smooth' });
}

// Handle event form submission (create or update)
async function handleEventFormSubmit(e) {
    e.preventDefault();
    
    try {
        // Get form data
        const eventId = document.getElementById('event-id').value;
        const eventData = {
            name: document.getElementById('event-name').value,
            description: document.getElementById('event-description').value,
            category: document.getElementById('event-category').value,
            date: new Date(document.getElementById('event-date').value).toISOString(),
            venue: document.getElementById('event-venue').value,
            price: parseFloat(document.getElementById('event-price').value),
            imageUrl: document.getElementById('event-image-url').value
        };
        
        let response;
        if (eventId) {
            // Update existing event
            eventData.id = parseInt(eventId);
            response = await API.Events.updateEvent(eventId, eventData);
        } else {
            // Create new event
            response = await API.Events.createEvent(eventData);
        }
        
        // Hide form
        document.getElementById('event-form-container').classList.add('d-none');
        
        // Reload events table
        loadEventsForAdmin();
        
        // Show success message
        alert(eventId ? 'Event updated successfully' : 'Event created successfully');
    } catch (error) {
        console.error('Error saving event:', error);
        alert(`Error saving event: ${error.message}`);
    }
}

// Cancel form submission
function cancelEventForm() {
    document.getElementById('event-form-container').classList.add('d-none');
}

// Initialize admin panel
document.addEventListener('DOMContentLoaded', () => {
    const currentPage = window.location.pathname.split('/').pop();
    
    if (currentPage === 'admin-panel.html') {
        // Ensure user is admin
        if (!requireAdmin()) return;
        
        // Load events for admin table
        loadEventsForAdmin();
        
        // Setup event listeners
        document.getElementById('create-event-btn').addEventListener('click', showCreateEventForm);
        document.getElementById('event-form').addEventListener('submit', handleEventFormSubmit);
        document.getElementById('cancel-event-btn').addEventListener('click', cancelEventForm);
    }
});