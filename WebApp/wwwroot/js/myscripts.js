let firstButton = null;
let secondButton = null;

// Function to handle button clicks
function handleClick(buttonId) {
    const command = document.getElementById("pieceCommand");

    if (firstButton !== null && secondButton !== null) {
        const oldCoords = document.getElementsByName("FirstPieceCoordinates")[0];
        if (oldCoords != null) {
            oldCoords.value = "";
        }
        const newCoords = document.getElementsByName("SecondPieceCoordinates")[0];
        if (newCoords != null) {
            newCoords.value = "";
        }
        if (command != null) {
            command.value = "";
        }
        firstButton.classList.remove("selected");
        secondButton.classList.remove("selected");
        firstButton = null;
        secondButton = null;
    } else if (firstButton === null) {
        // This is the first click
        firstButton = document.getElementById(buttonId);
        const firstCoords = document.getElementsByName("FirstPieceCoordinates")[0];
        if (firstCoords != null) {
            firstCoords.value = buttonId;
            firstButton.classList.add("selected");
        }
        if (command != null) {
            command.value = "PlaceAPiece";
        }
    } else if (secondButton === null) {
        // This is the second click
        secondButton = document.getElementById(buttonId);
        const secondCoords = document.getElementsByName("SecondPieceCoordinates")[0];
        if (secondCoords != null) {
            secondCoords.value = buttonId;
            secondButton.classList.add("selected");
        }
        if (command != null) {
            command.value = "RelocateAPiece";
        }

    }
}

// JavaScript function to trigger form submission
function submitForm() {
    document.getElementById('gameBoardForm').submit(); 
}

// Add event listeners to the buttons
document.addEventListener("DOMContentLoaded", function () {
    const elements = document.getElementsByName("pieceButton");
    for (let i = 0; i < elements.length; i++) {
        elements[i].addEventListener('click', function () {
            handleClick(elements[i].id)
        });
    }
});