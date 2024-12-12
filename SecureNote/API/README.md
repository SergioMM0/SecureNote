When running the API, you must run first the following command to create the database

```bash
dotnet ef database update
```

The Docker container can be created from the root of the ```SecureNote``` solution by running the following command:
```bash
docker build -f API/Dockerfile -t api .
```
