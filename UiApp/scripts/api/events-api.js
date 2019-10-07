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

export function getEvent(eventId) {
    return fetch(`/api/events/${eventId}`).then(response => response.json());
}

export function getGuests(eventId) {
    return fetch(`/api/events/${eventId}/guests`).then(response => response.json());
}

export function uploadGuestsList(eventId, file) {
    const data = new FormData();
    data.append('file', file);
    return fetch(`/api/events/${eventId}/guests-csv`, {
        method: 'PUT',
        body: data,
    }).then(response => response.json());
}