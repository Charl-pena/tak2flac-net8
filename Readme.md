
---

# tak2flac-net8

Este proyecto es un intento de portar la librería archivada [tak2flac](https://github.com/appleneko2001/tak2flac) a .NET 8. El objetivo principal es convertir archivos **TAK** a **FLAC**, utilizando **ffmpeg** como el componente subyacente que realiza la transformación.

## Descripción

La funcionalidad de **tak2flac-net8** está basada en el uso de **ffmpeg** para llevar a cabo la conversión de archivos TAK a FLAC. Dado que la implementación depende completamente de **ffmpeg**, si este no es capaz de transformar un archivo, la herramienta no puede ofrecer una solución alternativa.

Este proyecto fue un esfuerzo por modernizar la antigua librería tak2flac para el ecosistema .NET actual, pero hay varias limitaciones y problemas conocidos que limitan su fiabilidad.

## Problemas Conocidos

- **Manejo de nombres de archivos:** Si los nombres de los archivos contienen comillas o no están correctamente escapados, el comportamiento de la aplicación puede ser impredecible o fallar sin un manejo adecuado de errores.
- **Dependencia en ffmpeg:** Dado que toda la lógica de conversión depende de ffmpeg, cualquier limitación o fallo en ffmpeg afecta directamente al éxito de la conversión. Si **ffmpeg** no puede convertir un archivo TAK a FLAC, **tak2flac-net8** tampoco podrá hacerlo.
- **Errores no manejados:** Existen escenarios en los que errores no son capturados adecuadamente, lo que puede resultar en salidas inesperadas o fallos durante la ejecución.

## Estado del Proyecto

El proyecto no alcanzó los objetivos esperados debido a la complejidad y limitaciones inherentes en la dependencia de **ffmpeg**. Sin embargo, dado el esfuerzo significativo invertido en su desarrollo, se ha decidido conservar el código en este repositorio.

Este código podría servir como base para futuros intentos o actualizaciones, aunque se recomienda precaución si se pretende utilizar en entornos de producción debido a sus limitaciones actuales.

## Requisitos

- **.NET 8**
- **ffmpeg**: Asegúrate de tener instalado ffmpeg y que esté accesible desde la línea de comandos.
- **ffprobe**: Asegúrate de tener instalado ffprobe y que esté accesible desde la línea de comandos.
- Definir estas variables  
  ```sh
    FFMPEG_BINARY="Path of ffmpeg binary"
    FFPROBE_BINARY="Path of ffprobe binary"
  ```
## Uso

Para convertir un archivo TAK a FLAC:

```bash
tak2flac-net8 <input_file.tak> <output_file.flac>
```

Nota: Si se encuentra con errores relacionados con el nombre de archivos o si la conversión falla, verifique si el problema está relacionado con las limitaciones conocidas mencionadas anteriormente.

## Licencia

Este proyecto no tiene licencia asignada, ya que se trata de un experimento que no alcanzó los resultados esperados. No se recomienda su uso fuera de entornos de prueba o evaluación.

## Agradecimientos

El desarrollo de este proyecto está inspirado en el trabajo de la librería original [tak2flac](https://github.com/appleneko2001/tak2flac), ahora archivada.

---