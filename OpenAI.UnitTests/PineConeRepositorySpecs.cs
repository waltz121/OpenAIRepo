using OpenAiCore;
using OpenAiCore.OpenAiRepository;
using OpenAiCore.OpenAiRepository.DTO.OpenAi;
using OpenAiCore.OpenAiRepository.DTO.PineCone;
using OpenAiCore.OpenAiRepository.Model;
using OpenAiCore.PineConeRepository;
using System.Text.Json;

namespace OpenAI.UnitTests
{
    [TestClass]
    public class PineConeRepositorySpecs
    {
        private const string SampleEmbeddedText = "Glutamate is the primary excitatory neurotransmitter and GABA the main inhibitory neurotransmitter. These two are always working in conjunction with each other to maintain a balance. Glutamate converts into GABA in your brain. If you have trouble converting glutamate into GABA, you’ll have excitatory-like symptoms, as the glutamate over-accumulates";
        PineConeRepository PineConeRepository;
        OpenAiAPIRepository OpenAiAPIRepository;
        public PineConeRepositorySpecs()
        {
            Config.Init("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", @"C:\Users\walterr\Desktop\C#Apps\OpenAIApps\OpenAiCore\Files\EmbeddedOpenAiDataset.csv", "1000f1c9-9a38-471a-bdc5-483957668b0d");
            PineConeRepository = new PineConeRepository();
            OpenAiAPIRepository = new OpenAiAPIRepository();
        }

        [TestMethod]
        public void Upsert_SingleItem()
        {
            // Get Embeddings fromOpenAiApiRepository
            EmbeddingRequestDTO embeddingRequestDTO = new EmbeddingRequestDTO()
            {
                Input = [SampleEmbeddedText],
                Model = "text-embedding-ada-002"
            };
            EmbeddingResponseDTO embeddingResponseDTO = new EmbeddingResponseDTO();

            Task.Run(async () =>
            {
                embeddingResponseDTO = await OpenAiAPIRepository.CreateEmbeddings(embeddingRequestDTO);
            }).GetAwaiter().GetResult();

            var QueryEmbedding = embeddingResponseDTO.Data[0].Embedding;

            // Save Embeddings to PineCone
            PineConeUpsertRequestDTO requestBody = new PineConeUpsertRequestDTO()
            {
                Vectors = new List<PineConeVectorsDTO>
                 {
                    new PineConeVectorsDTO() {
                        ID = Guid.NewGuid().ToString(),
                        Values = QueryEmbedding,
                        Metadata =
                             new PineConeMetaDataDTO() {  Url = "www.mercola.com", Text=SampleEmbeddedText }
                            }
                 }

            };

            Task.Run(async () =>
                {
                    var response = await PineConeRepository.Upsert(requestBody);
                }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void Upsert_BatchProcess_FromCSV()
        {
            // Get Embeddings From CSV
            var lines = File.ReadAllLines(Config.OutputDataSet);
            var dataRecords = lines.Skip(1).Select(line =>
            {
                var columns = line.Split("|");
                return new EmbeddingCSVDataFrame
                {
                    url = columns[0],
                    text = columns[1],
                    embedding = JsonSerializer.Deserialize<List<float>>(columns[2])
                };
            }).ToList();

            PineConeUpsertRequestDTO requestDTO = new PineConeUpsertRequestDTO();

            // Save Embeddings to PineCone
            foreach (var row in dataRecords)
            {
                PineConeVectorsDTO vector = new PineConeVectorsDTO()
                {
                    ID = Guid.NewGuid().ToString(),
                    Values = row.embedding,
                    Metadata = new PineConeMetaDataDTO() { Url = row.url, Text = row.text }
                };

                requestDTO.Vectors.Add(vector);
            }

            Task.Run(async () =>
            {
                var response = await PineConeRepository.Upsert(requestDTO);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void Query_FromPineCone()
        {
            // Get Embeddings of Input
            EmbeddingRequestDTO embeddingRequestDTO = new EmbeddingRequestDTO()
            {
                Input = new List<string>()
                {
                    "What does having low vitamin D Means?"
                },
                Model = "text-embedding-ada-002"
            };
            EmbeddingResponseDTO embeddingResponseDTO = new EmbeddingResponseDTO();

            Task.Run(async () =>
            {
                embeddingResponseDTO = await OpenAiAPIRepository.CreateEmbeddings(embeddingRequestDTO);
            }).GetAwaiter().GetResult();

            var QueryEmbedding = embeddingResponseDTO.Data[0].Embedding;

            // Query PineCone using QueryEmbeddings
            PineConeQueryRequestDTO requestDTO = new PineConeQueryRequestDTO()
            {
                TopK = 10,
                Namespace = "ChatBotApp",
                IncludeValues = "false",
                IncludeMetadata = "true",
                Vector = QueryEmbedding
            };

            Task.Run(async () =>
            {
                var response = await PineConeRepository.Query(requestDTO);
            }).GetAwaiter().GetResult();

        }

        [TestMethod]
        public void Query_FromPineCone_WithMetaData()
        {
            // Get Embeddings of Input
            EmbeddingRequestDTO embeddingRequestDTO = new EmbeddingRequestDTO()
            {
                Input = new List<string>()
                {
                    "https://products.mercolamarket.com/vitamin-c/"
                },
                Model = "text-embedding-ada-002"
            };
            EmbeddingResponseDTO embeddingResponseDTO = new EmbeddingResponseDTO();
            Task.Run(async() =>
            {
                embeddingResponseDTO = await OpenAiAPIRepository.CreateEmbeddings(embeddingRequestDTO);
            }).GetAwaiter().GetResult();

            var QueryEmbedding = embeddingResponseDTO.Data[0].Embedding;

            // Query PineCone using QueryEmbeddings
            PineConeQueryRequestDTO requestDTO = new PineConeQueryRequestDTO()
            {
                TopK = 10,
                Namespace = "ChatBotApp",
                IncludeValues = "false",
                IncludeMetadata = "true",
                Vector = QueryEmbedding,
                Filter = new PineConeQueryFilterDTO()
                {
                    Url = "https://products.mercolamarket.com/vitamin-c/"
                }
            };

            Task.Run(async () =>
            {
                var response = await PineConeRepository.Query(requestDTO);
            }).GetAwaiter().GetResult();

        }
    }
}
