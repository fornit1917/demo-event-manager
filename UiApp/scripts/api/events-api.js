export function getEvents() {
    return fetch("/api/events").then(response => response.json());
}

export function createEvent(data) {
    return fetch("/api/events", {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify(data),
    }).then(response => response.json());
}

export function markEventAsArchive(eventId) {
    return fetch(`/api/events/${eventId}/archive`, {
        method: "PUT"
    });
}