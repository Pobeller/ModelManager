import sys
from diffusers import DiffusionPipeline

def download_model(model_name):
    try:
        pipe = DiffusionPipeline.from_pretrained(model_name)
        print(f'Modell {model_name} erfolgreich heruntergeladen.')
    except Exception as e:
        print(f'Fehler beim Herunterladen des Modells {model_name}: {str(e)}')

if __name__ == '__main__':
    if len(sys.argv) != 2:
        print('Verwendung: python download_model.py <Modellname>')
        sys.exit(1)
    download_model(sys.argv[1])
