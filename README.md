# Chronos

"**Chronos** is the personification of time in Greek mythology."

With **Chronos**, become the god of time for your Rust server. Simply set a duration for day and night, and the day/night cycle will be adjusted to match using the same curve system used by Vanilla Rust.

## Configuration
```
{
  "DAY_LENGTH": 45,
  "NIGHT_LENGTH": 15,
}
```
**`DAY_LENGTH`:**
The real time of a day in Rust. This spans from roughly the end of sunrise/dawn to the beginning of sunset/dusk.

**`NIGHT_LENGTH`:**
The real time of a night in Rust. This spawns from roughly the end of sunset/dusk to the beginning of sunrise/dawn.
