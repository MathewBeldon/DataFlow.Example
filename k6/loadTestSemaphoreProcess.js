import http from 'k6/http';
import { check } from 'k6';

export const options = {
    vus: 5,
    duration: '60s',
};

export default function () {
    const res = http.get('http://localhost:5000/api/Parallel/semaphore/10');
    check(res, {
        'is status 200': (r) => r.status === 200,
    });
};