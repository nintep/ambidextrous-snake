# Ambidextrous Snake
*A small project for testing Unity Test Framework and Unity build automation with GitHub Actions.*

![screenshot](/DemoImages/readme_header.gif)

## Project description
Ambidextrous Snake is a simple snake game where you control one snake with your left hand and another one with your right. I created the project to test how Unity's testing framework could be used in combination with GitHub's CI features.

The project currently contains a [workflow](https://github.com/nintep/ambidextrous-snake/actions/workflows/build-test.yml) with two jobs: 
- Automated Windows builds with [Unity - Builder](https://github.com/marketplace/actions/unity-builder) action by GameCI.
- Automated PlayMode and EditMode test runs with [Unity - Test runner](https://github.com/marketplace/actions/unity-test-runner) action by GameCI.

---

[![Unity](https://img.shields.io/badge/Unity_version-2022.3.20f1-green)](https://unity.com/)

[Project Github](https://github.com/nintep/ambidextrous-snake)
