﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace MxNet.GluonCV.ModelZoo
{
    public class ModelStore
    {
        private static readonly Dictionary<string, string> model_sha1 = new Dictionary<string, string>
        {
            {"alexnet", "44335d1f0046b328243b32a26a4fbd62d9057b45"},
            {"densenet121", "f27dbf2dbd5ce9a80b102d89c7483342cd33cb31"},
            {"densenet161", "b6c8a95717e3e761bd88d145f4d0a214aaa515dc"},
            {"densenet169", "2603f878403c6aa5a71a124c4a3307143d6820e9"},
            {"densenet201", "1cdbc116bc3a1b65832b18cf53e1cb8e7da017eb"},
            {"inceptionv3", "a5050dbcbfc54908fc1b7700698eb7ffbc150417"},
            {"xception", "37c1c90b56800303a66934487fbf017bca8bba00"},
            {"mobilenet0.25", "f0046a3da4e150b85da58ea31913dbb86e7540d1"},
            {"mobilenet0.5", "0130d2aa2f7e3a63db35579b243e25f512172371"},
            {"mobilenet0.75", "84c801e27b1ac5040b6e945c0d1f8d49e9a59f3d"},
            {"mobilenet1.0", "efbb2ca3881998d5a8c5cb6c457a28c1e085fed5"},
            {"mobilenetv2_1.0", "f9952bcde7a7982947ac5240546e1c1c8f057370"},
            {"mobilenetv2_0.75", "b56e3d1c33eb52d0b90678db4ce6c5ca6c9a6704"},
            {"mobilenetv2_0.5", "0803818513599fa1329524ee3607b708b4a4630f"},
            {"mobilenetv2_0.25", "9b1d2cc38fed4cd171a7f7a0d17fe1a905573887"},
            {"mobilenetv3_large", "eaa44578554ddffaf2a2630ced9093181ff79688"},
            {"mobilenetv3_small", "10430433698d18f49991e4a366c9fce8f9286298"},
            {"resnet18_v1", "a0666292f0a30ff61f857b0b66efc0228eb6a54b"},
            {"resnet34_v1", "48216ba99a8b1005d75c0f3a0c422301a0473233"},
            {"resnet50_v1", "cc729d95031ca98cf2ff362eb57dee4d9994e4b2"},
            {"resnet101_v1", "d988c13d6159779e907140a638c56f229634cb02"},
            {"resnet152_v1", "acfd09703b113143af9c33898bad8b6154fd6fb0"},
            {"resnet18_v2", "a81db45fd7b7a2d12ab97cd88ef0a5ac48b8f657"},
            {"resnet34_v2", "9d6b80bbc35169de6b6edecffdd6047c56fdd322"},
            {"resnet50_v2", "ecdde35339c1aadbec4f547857078e734a76fb49"},
            {"resnet101_v2", "18e93e4f48947e002547f50eabbcc9c83e516aa6"},
            {"resnet152_v2", "f2695542de38cf7e71ed58f02893d82bb409415e"},
            {"squeezenet1.0", "264ba4970a0cc87a4f15c96e25246a1307caf523"},
            {"squeezenet1.1", "33ba0f93753c83d86e1eb397f38a667eaf2e9376"},
            {"vgg11", "dd221b160977f36a53f464cb54648d227c707a05"},
            {"vgg11_bn", "ee79a8098a91fbe05b7a973fed2017a6117723a8"},
            {"vgg13", "6bc5de58a05a5e2e7f493e2d75a580d83efde38c"},
            {"vgg13_bn", "7d97a06c3c7a1aecc88b6e7385c2b373a249e95e"},
            {"vgg16", "e660d4569ccb679ec68f1fd3cce07a387252a90a"},
            {"vgg16_bn", "7f01cf050d357127a73826045c245041b0df7363"},
            {"vgg19", "ad2f660d101905472b83590b59708b71ea22b2e5"},
            {"vgg19_bn", "f360b758e856f1074a85abd5fd873ed1d98297c3"},
            {"vgg16_atrous", "4fa2e1ad96b8c8d1ba9e5a43556cd909d70b3985"},
            {"ssd_300_vgg16_atrous_voc", "0e169fbb64efdee6985c3c175ec4298c4bda0298"},
            {"ssd_512_vgg16_atrous_voc", "daf8181b615b480236fcb8474545077891276945"},
            {"ssd_512_resnet50_v1_voc", "9c8b225a552614e4284a0f647331bfdc6940eb4a"},
            {"ssd_512_resnet101_v2_voc", "2cc0f93edf1467f428018cc7261d3246dfa15259"},
            {"ssd_512_mobilenet1.0_voc", "37c180765a4eb3e67751d6bacac47bb9156f5fff"},
            {"ssd_300_vgg16_atrous_coco", "b302ad8a8660345c368448141d8acf30b5a3801d"},
            {"ssd_512_vgg16_atrous_coco", "5c86064290c05eccbdd88475376c71c595c8325c"},
            {"ssd_512_resnet50_v1_coco", "c48351620d4f0cbc49e4f7a84c8e67ef8fdc6e09"},
            {"ssd_512_mobilenet1.0_coco", "da9756faa5b9b4e34dedcf83ee0733d5895796ad"},
            {"faster_rcnn_resnet50_v1b_voc", "447328d89d70ae1e2ca49226b8d834e5a5456df3"},
            {"faster_rcnn_resnet50_v1b_coco", "5b4690fb7c5b62c44fb36c67d0642b633697f1bb"},
            {"faster_rcnn_resnet101_v1d_coco", "a465eca35e78aba6ebdf99bf52031a447e501063"},
            {"faster_rcnn_fpn_resnet50_v1b_coco", "233572743bc537291590f4edf8a0c17c14b234bb"},
            {"faster_rcnn_fpn_bn_resnet50_v1b_coco", "977c247d70c33d1426f62147fc0e04dd329fc5ec"},
            {"faster_rcnn_fpn_resnet101_v1d_coco", "1194ab4ec6e06386aadd55820add312c8ef59c74"},
            {"mask_rcnn_resnet18_v1b_coco", "e071cf1550bc0331c218a9072b59e9550595d1e7"},
            {"mask_rcnn_resnet50_v1b_coco", "a3527fdc2cee5b1f32a61e5fd7cda8fb673e86e5"},
            {"mask_rcnn_resnet101_v1d_coco", "4a3249c584f81c2a9b5d852b742637cd692ebdcb"},
            {"mask_rcnn_fpn_resnet18_v1b_coco", "14a922c38b0b196fdb1f7141be4666c10476f426"},
            {"mask_rcnn_fpn_resnet50_v1b_coco", "1364d0afe4de575af5d4389d50c2dbf22449ceac"},
            {"mask_rcnn_fpn_resnet101_v1d_coco", "89c7d8669b677a05c6eaa25375ead9a174109c69"},
            {"cifar_resnet20_v1", "121e1579d811b091940b3b1fa033e1f0d1dca40f"},
            {"cifar_resnet20_v2", "4f2d18804c94f2d283b8b45256d048bd3d6dd479"},
            {"cifar_resnet56_v1", "2fb251e60babdceb103e9659b3baa0dea20a14d7"},
            {"cifar_resnet56_v2", "0a3e74104ec7bcfffefe2d9d5cc1f8e74311ec51"},
            {"cifar_resnet110_v1", "a0e1f860475bf5369f6da07e0c2e03a4ae9cff9c"},
            {"cifar_resnet110_v2", "bf160f8b3cb3884a1ea871739f3c8e151e114159"},
            {"cifar_wideresnet16_10", "7c07b5ba6e850f9c37ca1e57c0a2e529455cc2e4"},
            {"cifar_wideresnet28_10", "4a3466aadd4c3ddbcb968bca862d0e59d6f15ec1"},
            {"cifar_wideresnet40_8", "085ca2afabbe0ddfe87d0edc5408bcfcfbffd414"},
            {"cifar_resnext29_16x64d", "e8ff9f4f9cb319dfbf524d01e487af9a7f8a3cf5"},
            {"resnet18_v1b", "2d9d980c990442f826f20781ed039851e78dabe3"},
            {"resnet34_v1b", "8e16b84814e84f64d897854003f049872991eaa6"},
            {"resnet50_v1b", "0ecdba34691be172036ddf244ff1b2eade75ffde"},
            {"resnet50_v1b_gn", "48ddf358d5acc879f76740dae695be67d96beea6"},
            {"resnet101_v1b", "a455932aa95cb7dcfa05fd040b9b5a5660733c39"},
            {"resnet152_v1b", "a5a61ee1ce5ab7c09720775b223360f3c60e211d"},
            {"resnet50_v1c", "2a4e070854db538595cc7ee02e1a914bdd49ca02"},
            {"resnet101_v1c", "064858f23f9878bfbbe378a88ccb25d612b149a1"},
            {"resnet152_v1c", "75babab699e1c93f5da3c1ce4fd0092d1075f9a0"},
            {"resnet50_v1d", "117a384ecf61490eb31ea147eb0e61e6d2b8a449"},
            {"resnet101_v1d", "1b2b825feff86b0354642a4ab59f9b6e35e47338"},
            {"resnet152_v1d", "cddbc86ff24a5544f57242ded0acb14ef1fbd437"},
            {"resnet50_v1s", "25a187fa281ddc98afbcd0cc0f0646885b874b80"},
            {"resnet101_v1s", "bd93a83c05f709a803b1221aeff0b028e6eebb03"},
            {"resnet152_v1s", "cf74621d988ad06c6c6aa44f5597e5b600a966cc"},
            {"fcn_resnet101_coco", "766cdf9cc3e5b980b141643f054db5b48863f634"},
            {"fcn_resnet101_voc", "12c2b9b3be7d4e133e52477150a9b3e616626a82"},
            {"fcn_resnet50_ade", "3479525af7bdbf345e74e150aaae2e48174c0c5f"},
            {"fcn_resnet101_ade", "d544440a35586f662ed1a5405ab9aa89cd750558"},
            {"deeplab_resnet101_coco", "ed817f76086abb4c3404af62ec1b5487c67642b7"},
            {"deeplab_resnet101_voc", "311ed22c63f3ac28b5f1e1663c458f26600e62da"},
            {"deeplab_resnet152_coco", "83247aaeeded6988790cd93ac93151514094846f"},
            {"deeplab_resnet152_voc", "d35bea8817935d1ab310ef1e6dd06bb18c2d5f0d"},
            {"deeplab_resnet50_ade", "c7789b237adc7253405bee57c84d53b15db45942"},
            {"deeplab_resnet101_ade", "bf1584dfcec12063eff3075ee643e181c0f6d443"},
            {"psp_resnet101_coco", "09f89cad0e107cb2bffdb1b07706ba31798096f2"},
            {"psp_resnet101_voc", "2c2f4e1c2b11461b52598a4b2038bccbcfc166eb"},
            {"psp_resnet50_ade", "3f220f537400dfa607c3d041ed3b172db39b0b01"},
            {"psp_resnet101_ade", "240a4758b506447faf7c55cd7a7837d66f5039a6"},
            {"psp_resnet101_citys", "d0e8603120ab02118a8973d52a26b8296d1b4078"},
            {"deeplab_v3b_plus_wideresnet_citys", "ef2bb40ad8f8f59f451969b2fabe4e548394e80a"},
            {"yolo3_darknet53_voc", "f5ece5ce1422eeca3ce2908004e469ffdf91fd41"},
            {"yolo3_mobilenet1.0_voc", "3b47835ac3dd80f29576633949aa58aee3094353"},
            {"yolo3_mobilenet1.0_coco", "66dbbae67be8f1e3cd3c995ce626a2bdc89769c6"},
            {"yolo3_darknet53_coco", "09767802230b45af1c27697a2dad6d1ebaacc1e2"},
            {"darknet53", "2189ea49720a116dead245b9b252301cffa18d28"},
            {"senet_154", "b5538ef10557243511b9b46063aa4c40790d74ba"},
            {"resnext50_32x4d", "4ecf62e29336e0cbc5a2f844652635a330928b5a"},
            {"resnext101_32x4d", "8654ca5d0ba30a7868c5b42a7d4cc0ff2ba04dbc"},
            {"resnext101_64x4d", "2f0d1c9d343d140775bfa7548dd3a881a35855de"},
            {"se_resnext50_32x4d", "7906e0e16013ef8d195cbc05463cc37783ec7a8a"},
            {"se_resnext101_32x4d", "688e238985d45a38803c62cf345af2813d0e8aa0"},
            {"se_resnext101_64x4d", "11c50114a0483e27e74dc4236904254ef05b634b"},
            {"simple_pose_resnet18_v1b", "f63d42ac8f83b239d4e08b636b888b8e50cd066d"},
            {"simple_pose_resnet50_v1b", "e2c7b1adea31264bc9220511308b4efa89c6fc50"},
            {"simple_pose_resnet101_v1b", "b7ec0de1a34eb718efd4a84339cc1547ead88cbe"},
            {"simple_pose_resnet152_v1b", "ef4e033612a5fca6fc69e54c87da3ba3866d533e"},
            {"simple_pose_resnet50_v1d", "ba2675b6a43fc31601f0e99311b0bb115369bc82"},
            {"simple_pose_resnet101_v1d", "1f8f48fd49a23bcc73c1cd736bdc639cd1434489"},
            {"simple_pose_resnet152_v1d", "3ca502ea8eaaa15f4f972d5cf139167d15ffa798"},
            {"mobile_pose_resnet18_v1b", "dd6644ebca0d320eb460342d1ed6e1f0793e3946"},
            {"mobile_pose_resnet50_v1b", "ec8809df9adbeefc022e5977fda60c4e4a58c3ac"},
            {"mobile_pose_mobilenet1.0", "b399bac75ebbd1b9d04a16906bddc64f1e33496b"},
            {"mobile_pose_mobilenetv2_1.0", "4acdc130ebee695b1181fb7e4eee8f2c4d91a614"},
            {"mobile_pose_mobilenetv3_large", "1ca004dc5ab2bd0e10d782734d3efbefff23180c"},
            {"mobile_pose_mobilenetv3_small", "c2a11fae8970c2c2e79e2b77a4c4d62e3d3e054f"},
            {"resnet18_v1b_2.6x", "54f7742b1f8939ef8e59ede3469bfa5eb6e247fa"},
            {"resnet50_v1d_1.8x", "a230c33f7966ab761597328686b28d0545e4ea30"},
            {"resnet50_v1d_3.6x", "0d3e69bb033d1375c3734419bbc653c3a474ea53"},
            {"resnet50_v1d_5.9x", "9982ae4985b14e1c0ab25342a9f08bc4773b3998"},
            {"resnet50_v1d_8.8x", "6a25eeceb7d27bd9c05fa2bf250c55d3960ad4c7"},
            {"resnet101_v1d_1.9x", "a872796b63fb883116831db3454711421a628154"},
            {"resnet101_v1d_2.2x", "712fccb185921a596baebe9246ff6c994b88591b"},
            {"alpha_pose_resnet101_v1b_coco", "de56b871543847d586deeca488b5bfe1b77bb5c5"},
            {"googlenet", "c7c89366fb4410c0aeb34827795f7dab9423f950"},
            {"vgg16_ucf101", "d6dc1bbaaf3cbe0be19f02362706393f62ce76fa"},
            {"inceptionv1_kinetics400", "6dcdafb1dd19866fdd3116cbe3689adb85824b2c"},
            {"inceptionv3_ucf101", "13ef5c3bd40141f11c5b4e471f8f2ad0c1ac6299"},
            {"inceptionv3_kinetics400", "8a4a6946893de23937b5e5e4a1a449800d57ff71"},
            {"resnet18_v1b_kinetics400", "46d5a9850cdb884eb9c9a95d48269c3d60c94d77"},
            {"resnet34_v1b_kinetics400", "8a8d0d8d395882f836d379aefb4136dce8763d0c"},
            {"resnet50_v1b_hmdb51", "682591e23ce4b92fbd3222c0710ebb52166fddca"},
            {"resnet50_v1b_kinetics400", "cc757e5c94fdaaea64a8cb5acfbd655ddf6ffa96"},
            {"resnet50_v1b_sthsthv2", "80ee0c6bef8b0240ec88273531cd7f43e3f6b65d"},
            {"resnet101_v1b_kinetics400", "5bb6098ea5343dc0f4bf8536b3771b8ddfe231d7"},
            {"resnet152_v1b_kinetics400", "9bc70c66059854a22e935b42f783ef2bec0377d8"},
            {"i3d_inceptionv1_kinetics400", "81e0be1043ea4c68a4a0a439918c7b888ee5545b"},
            {"i3d_inceptionv3_kinetics400", "f14f8a99007200ef56521b6a844f662784a8f5de"},
            {"i3d_resnet50_v1_ucf101", "760d0981094787b8789ee4a8c382d09d493c7413"},
            {"i3d_resnet50_v1_hmdb51", "2ec6bf01a55af38579380e6531d0ecc816862abe"},
            {"i3d_resnet50_v1_kinetics400", "568a722eb61da663e11b582886ddbef9ef8f6ac6"},
            {"i3d_resnet50_v1_sthsthv2", "01961e4cccf6405cd1342670b9525c21c578c9d4"},
            {"i3d_resnet101_v1_kinetics400", "6b69f655c60823bd05a83fe076c61d6c297add0d"},
            {"i3d_nl5_resnet50_v1_kinetics400", "3c0e47ea5ee699c3e1f706c9df7a74dbd6321b11"},
            {"i3d_nl10_resnet50_v1_kinetics400", "bfb58c4127705ad6e98f4916abde0c849e2f1288"},
            {"i3d_nl5_resnet101_v1_kinetics400", "fbfc1d30d90c304295dedd6c70b037d100e43d5f"},
            {"i3d_nl10_resnet101_v1_kinetics400", "59186c31dea2f20940a358fc8ea5199cd6d4303c"},
            {"slowfast_4x16_resnet50_kinetics400", "9d650f5186ffc08348f8d7384d6994a8f39a03b1"},
            {"slowfast_8x8_resnet50_kinetics400", "d6b253398615a21d8b2a827ddfb09c0d8827f79c"},
            {"slowfast_8x8_resnet101_kinetics400", "fbde1a7cfdaeeba7190dd15f284b91036bf5f3f6"},
            {"dla34", "5fd8d651eea4b8f3767174ea63bd0afc1fa541d8"},
            {"center_net_resnet18_v1b_voc", "38c509d456a6e14e4c54e961ed43bffe8cf62840"},
            {"center_net_resnet18_v1b_dcnv2_voc", "04d1ed20ae3f8a150bc06d8007775a6df99e40b2"},
            {"center_net_resnet50_v1b_voc", "f108427bc62c85f7bcbdba45db4c94a31fd4d4f6"},
            {"center_net_resnet50_v1b_dcnv2_voc", "61eb866d36ef68b1145a74a5b4e76ba262dbb4e4"},
            {"center_net_resnet101_v1b_voc", "5bf8b91f8813e82a2f2660c83560ffdfebf835a0"},
            {"center_net_resnet101_v1b_dcnv2_voc", "a0e707225164fc578b5bb7187a79e6b492da8fb4"},
            {"center_net_resnet18_v1b_coco", "dccae71d1f069343326750bc9b0508a2a760dd80"},
            {"center_net_resnet18_v1b_dcnv2_coco", "0874df9a5236297fd32ed401f0a699602ee6b42b"},
            {"center_net_resnet50_v1b_coco", "28c64aaeaf9d5e4404afd2b96bf8812973d79eb9"},
            {"center_net_resnet50_v1b_dcnv2_coco", "2713a7ba29ab4da5f1939da5a53f97ed079441ca"},
            {"center_net_resnet101_v1b_coco", "6f9cd4a945cb554c38539009eede20f45786519e"},
            {"center_net_resnet101_v1b_dcnv2_coco", "52daf2d9faca82c08924d08e7e0c253a782a1653"}

        };

        private static readonly string apache_repo_url = "https://apache-mxnet.s3-accelerate.dualstack.amazonaws.com/";
        private static readonly string _url_format = "{0}gluon/models/{1}.zip";

        public static string ShortHash(string name)
        {
            if (model_sha1.ContainsKey(name))
                return model_sha1[name].Substring(0, 8);

            throw new Exception($"Pretrained model for {name} is not available");
        }

        public static string GetModelFile(string name, string tag = "", string root = "")
        {
            if (string.IsNullOrWhiteSpace(root))
                root = mx.AppPath + "\\Models";

            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);

            var file_name = $"{name}-{ShortHash(name)}";
            if (!string.IsNullOrWhiteSpace(tag))
            {
                file_name = $"{name}-{tag}";
            }

            var file_path = $"{root}\\{file_name}.params";
            var shal1_hash = model_sha1[name];
            if (File.Exists(file_path))
            {
                if (MxNet.Gluon.Utils.CheckSha1(file_path, shal1_hash))
                    return file_path;
                Logger.Warning("Mismatch in the content of model file detected. Downloading again.");
            }
            else
            {
                Logger.Info($"Model file not found. Downloading to {file_path}");
            }

            var zip_file_path = Path.Combine(root, file_name + ".zip");
            var repo_url = Environment.GetEnvironmentVariable("MXNET_GLUON_REPO");
            if (string.IsNullOrWhiteSpace(repo_url))
                repo_url = apache_repo_url;

            if (!repo_url.EndsWith("/"))
                repo_url += "/";

            MxNet.Gluon.Utils.Download(string.Format(_url_format, repo_url, file_name), zip_file_path, true);
            ZipFile.ExtractToDirectory(zip_file_path, root);
            File.Delete(zip_file_path);

            if (MxNet.Gluon.Utils.CheckSha1(file_path, shal1_hash))
                return file_path;
            throw new Exception("Downloaded file has different hash. Please try again.");
        }

        public static void Purge(string root = "")
        {
            if (string.IsNullOrWhiteSpace(root))
                root = mx.AppPath + "\\Models";
            var dir = new DirectoryInfo(root);
            var files = dir.GetFiles("*.params");
            foreach (var item in files) item.Delete();
        }

        public static string[] PretrainedModelList() => throw new NotImplementedException();
    }
}