// auth.js - Handle authentication state and operations



function isTokenExpired() {
  const expiry = localStorage.getItem("tokenExpiry");
  if (!expiry) return true; // إذا لم يكن هناك وقت انتهاء، يعتبر التوكن منتهيًا
  return Date.now() > parseInt(expiry); // التحقق إذا كان الوقت الحالي تجاوز وقت الانتهاء
}

// const API = {
//   Auth: {
//     register: async (userData) => {
//       const response = await fetch("/api/Auth/register", {
//         method: "POST",
//         headers: {
//           "Content-Type": "application/json",
//         },
//         body: JSON.stringify(userData),
//       });
//       return response.json(); // إرجاع الاستجابة ككائن JSON
//     },
//     login: async (credentials) => {
//       const response = await fetch("/api/Auth/login", {
//         method: "POST",
//         headers: {
//           "Content-Type": "application/json",
//         },
//         body: JSON.stringify(credentials),
//       });
//       return response.json(); // إرجاع الاستجابة ككائن JSON
//     },
//   },
// };

function isLoggedIn() {
  return localStorage.getItem("token") !== null && !isTokenExpired();
}

// // Check if user is logged in
// function isLoggedIn() {
//   return localStorage.getItem("token") !== null;
// }

// Check if user is admin
function isAdmin() {
  const roles = JSON.parse(localStorage.getItem("roles") || "[]");
  return roles.includes("Admin");
}

// Save user auth data to local storage
function saveAuthData(authData) {
  localStorage.setItem("token", authData.token);
  localStorage.setItem("userId", authData.userId);
  localStorage.setItem("email", authData.email);
  localStorage.setItem("roles", JSON.stringify(authData.roles || []));
  localStorage.setItem("tokenExpiry", Date.now() + 3600 * 1000); // علشان نتحقق من التوكن
}

// Clear auth data on logout
function logout() {
  localStorage.removeItem("token");
  localStorage.removeItem("userId");
  localStorage.removeItem("email");
  localStorage.removeItem("roles");
  // Redirect to login page
  window.location.href = "login.html";
}


async function register(firstName, lastName, email, password, confirmPassword) {
  try {
    const userData = {
      firstName,
      lastName,
      email,
      password,
      confirmPassword,
    };

    const response = await API.Auth.register(userData);
    if (response.isSuccess) {
      // Save auth data and redirect
      saveAuthData(response);
      window.location.href = "index.html";
    } else {
      throw new Error(response.errors?.join(", ") || "Registration failed");
    }
  } catch (error) {
    throw error;
  }
}



async function login(email, password) {
  try {
    const credentials = { email, password };

    const response = await API.Auth.login(credentials);
    if (response.isSuccess) {
      // Save auth data and redirect
      saveAuthData(response);

      // Redirect admin to admin panel, regular users to homepage
      if (isAdmin()) {
        window.location.href = "admin-panel.html";
      } else {
        window.location.href = "index.html";
      }
    } else {
      throw new Error(response.errors?.join(", ") || "Login failed");
    }
  } catch (error) {
    throw error;
  }
}


// Protect admin routes
function requireAdmin() {
  if (!isLoggedIn() || !isAdmin()) {
    window.location.href = "login.html";
    return false;
  }
  return true;
}

// Protect authenticated routes
function requireAuth() {
  if (!isLoggedIn()) {
    window.location.href = "login.html";
    return false;
  }
  return true;
}

// Update UI based on auth state
function updateAuthUI() {
  const authLinks = document.getElementById("auth-links");
  if (!authLinks) return;

  if (isLoggedIn()) {
    const email = localStorage.getItem("email");

    // Show different options based on role
    if (isAdmin()) {
      authLinks.innerHTML = `
                <li class="nav-item"><a class="nav-link" href="admin-panel.html">Admin Panel</a></li>
                <li class="nav-item"><span class="nav-link">${email}</span></li>
                <li class="nav-item"><a class="nav-link" href="#" id="logout-btn">Logout</a></li>
            `;
    } else {
      authLinks.innerHTML = `
                <li class="nav-item"><span class="nav-link">${email}</span></li>
                <li class="nav-item"><a class="nav-link" href="#" id="logout-btn">Logout</a></li>
            `;
    }

    // Add event listener to logout button
    document.getElementById("logout-btn").addEventListener("click", (e) => {
      e.preventDefault();
      logout();
    });
  } else {
    authLinks.innerHTML = `
            <li class="nav-item"><a class="nav-link" href="login.html">Login</a></li>
            <li class="nav-item"><a class="nav-link" href="register.html">Register</a></li>
        `;
  }
}

// Initialize auth state on page load
document.addEventListener("DOMContentLoaded", updateAuthUI);
