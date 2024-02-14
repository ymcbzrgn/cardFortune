## Card Fortune - Love Fortune Application

This README details the functionality and usage of **Card Fortune**, a love fortune application that utilizes a deck of cards and your partner's name to generate a personalized reading.

### Table of Contents

- [Features](#features)
- [How it Works](#how-it-works)
- [Usage](#usage)
- [Technical Details](#technical-details)
- [Contributing](#contributing)
- [License](#license)

### Features

- **Personalized Fortunes:** Generate unique readings based on the number of characters in your partner's name.
- **Interactive Card Selection:** Choose specific cards for key positions, influencing the fortune's outcome.
- **Meaningful Interpretations:** Discover the significance of chosen cards through associated meanings.
- **Deck Manipulation:** Witness the cards shuffle and distribute dynamically based on your input.

### How it Works

1. **Card Preparation:** The application retrieves a deck of cards from a database.
2. **Deck Shuffling:** The deck shuffles dynamically based on your chosen number of shuffles.
3. **Personalized Distribution:** Cards distribute into piles based on your partner's name length, creating a unique layout.
4. **Iterative Refinement:** Piles combine and cards are removed iteratively until a single pile remains.
5. **Special Card Selection:** Choose cards for specific positions like "Yourself" or "Him/Her," adding your personal touch.
6. **Meaningful Connections:** The application retrieves and associates card meanings based on their final positions.
7. **Personalized Reading:** Unveil your unique fortune, encompassing chosen cards and their interpreted meanings.

### Usage

1. Ensure you have MongoDB set up with the required collections (consult code for details).
2. Set environment variables (`MONGODB_CONNECTION_STRING`, etc.) with your database connection details.
3. Run the `program.cs` file.
4. Follow the on-screen instructions, providing your partner's name length and interacting with card selections.
5. Receive your personalized love fortune, presented with card combinations and their corresponding interpretations.

### Technical Details

- **Development Framework:** .NET Framework
- **Dependencies:** MongoDB.Driver, System.Runtime.CompilerServices
- **Data Storage:** MongoDB collections

### Contributing

We welcome contributions! Feel free to submit pull requests for improvements, new features, or code enhancements.

### License

This project is licensed under the MIT License. See the LICENSE file for details.
