window.onload = function() {
    const showCreatedPasswordBtn = document.getElementById("showCreatedPasswordBtn");
    showCreatedPasswordBtn.addEventListener("click", showPasswordWhenCreating);
}

function showPasswordWhenCreating() {
    const createdPassword = document.getElementById("createdPassword");
    const showCreatedPasswordBtn = document.getElementById("showCreatedPasswordBtn");
    if (createdPassword.type === "password") {
        createdPassword.type = "text";
        showCreatedPasswordBtn.textContent = "Hide password";
    } else {
        createdPassword.type = "password";
        showCreatedPasswordBtn.textContent = "Show password";
    }
}