import subprocess
import os

try:
    os.remove("Carpool.db") # needs to be same as EF database name in AddDbContext in Startup.cs
except OSError:
    pass

subprocess.run("dotnet ef migrations remove", shell=True)
subprocess.run("dotnet ef migrations add init", shell=True)
subprocess.run("dotnet ef database update", shell=True)