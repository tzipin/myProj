 submitLogin = async (event) => {
    event.preventDefault();
    const name = document.getElementById("name").value;
    const password = document.getElementById("password").value;

    const author = {
        Id: 0,
        Name: name,
        Password: password,
        Email: "",
        Address: "",
        Level: 0,
    };

    try {
        const response = await fetch('/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(author),
        });

        if (response.ok) {
            const result = await response.text();
            sessionStorage.setItem("token", result);
            window.location.href = "/";
        } else {
            const error = await response.text();
            //throw new Error(error);
            console.log(error);            
        }
    } catch (error) {
        console.error("Error during login:", error);
        alert("An error occurred. Please try again.");
    }
}