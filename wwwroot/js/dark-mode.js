// dark-mode.js
document.addEventListener("DOMContentLoaded", function () {
    const theme = localStorage.getItem('theme');
    if (theme === 'dark') {
        document.body.classList.add('dark-mode');
        updateDarkModeIcons(true);
    }
});

function toggleDarkMode(event) {
    if (event) {
        event.stopPropagation();
    }
    const isDark = document.body.classList.toggle('dark-mode');
    if (isDark) {
        localStorage.setItem('theme', 'dark');
    } else {
        localStorage.setItem('theme', 'light');
    }
    updateDarkModeIcons(isDark);
}

function updateDarkModeIcons(isDark) {
    // Tüm dark mode butonlarının ikonlarını günceller
    const icons = document.querySelectorAll('.dark-mode-icon');
    const texts = document.querySelectorAll('.dark-mode-text');
    
    icons.forEach(icon => {
        if (isDark) {
            icon.classList.remove('bi-moon');
            icon.classList.add('bi-sun');
        } else {
            icon.classList.remove('bi-sun');
            icon.classList.add('bi-moon');
        }
    });

    texts.forEach(text => {
        if (isDark) {
            text.innerText = text.getAttribute('data-light-label') || "Light Mode";
        } else {
            text.innerText = text.getAttribute('data-dark-label') || "Dark Mode";
        }
    });
}
