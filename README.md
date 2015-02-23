status
======

Status framework for a hackerspace. Stores informations in key / value pairs. The framework has three parts, the backend, the sensor and the frontends.

**Backend**

The backend is a php application which provides a REST api, to read, write, create and remove key value pairs in a MySQL database.

**Sensor**

The sensor is a application which detects status values, like device count.

**Frontends**

The framework supply a various number of frontends, like the signal light, a tweet status frontend et cetera. 