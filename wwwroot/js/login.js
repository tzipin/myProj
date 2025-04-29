 submitLogin = async (event) => {
    event.preventDefault(); // Prevent the default form submission

    // Get the name and password from the form
    const name = document.getElementById("name").value;
    const password = document.getElementById("password").value;

    // Create the Author object
    const author = {
        Id: 0,
        Name: name,
        Password: password,
        Email: "",
        Address: "",
        Level: 0,
    };

    try {
        // Send the POST request to the server
        const response = await fetch('/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(author),
        });

        // Handle the response
        if (response.ok) {
            const result = await response.text();
            window.location.href = "/"; // Redirect to the home page on success
        } else {
            const error = await response.text();
            alert("Login failed: " + error);
        }
    } catch (error) {
        console.error("Error during login:", error);
        alert("An error occurred. Please try again.");
    }
}