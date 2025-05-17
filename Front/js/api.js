const API_BASE_URL = 'https://localhost:5000'; 

async function handleResponse(response) {
  if (!response.ok) {
    const errorData = await response.json().catch(() => null);
    const errorMessage = errorData?.errors?.join(', ') || 'An error occurred';
    throw new Error(errorMessage);
  }
  return response.json();
}

const AuthAPI = {
  register: async (userData) => {
    const response = await fetch(`${API_BASE_URL}/auth/register`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(userData),
    });
    return handleResponse(response);
  },

  login: async (credentials) => {
    const response = await fetch(`${API_BASE_URL}/auth/login`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(credentials),
    });
    return handleResponse(response);
  },
};

const EventsAPI = {
  getAllEvents: async () => {
    const token = localStorage.getItem('token');
    const response = await fetch(`${API_BASE_URL}/events`, {
      headers: {
        'Authorization': `Bearer ${token}`,
      },
    });
    return handleResponse(response);
  },

};

const BookingsAPI = {
  getUserBookings: async () => {
    const token = localStorage.getItem('token');
    const response = await fetch(`${API_BASE_URL}/bookings/user`, {
      headers: {
        'Authorization': `Bearer ${token}`,
      },
    });
    return handleResponse(response);
  },
 
};


window.API = {
  Auth: AuthAPI,
  Events: EventsAPI,
  Bookings: BookingsAPI,
};
