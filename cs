/**
 * 更新日期：2024-04-05 15:30:15
 * 用法：Sub-Store 脚本操作添加
 * rename.js 参数说明请参考原始脚本注释
 */

// 获取输入参数
const inArg = $arguments; // console.log(inArg)
const nx = inArg.nx || false,
  bl = inArg.bl || false,
  nf = inArg.nf || false,
  key = false, // 强制关闭 key 过滤，避免没有国旗节点被过滤
  blgd = inArg.blgd || false,
  blpx = inArg.blpx || false,
  blnx = inArg.blnx || false,
  numone = inArg.one || false,
  debug = inArg.debug || false,
  clear = inArg.clear || false,
  addflag = inArg.flag || false,
  nm = true; // 保留未匹配节点

const FGF = inArg.fgf == undefined ? " " : decodeURI(inArg.fgf),
  XHFGF = inArg.sn == undefined ? " " : decodeURI(inArg.sn),
  FNAME = inArg.name == undefined ? "" : decodeURI(inArg.name),
  BLKEY = inArg.blkey == undefined ? "" : decodeURI(inArg.blkey),
  blockquic = inArg.blockquic == undefined ? "" : decodeURI(inArg.blockquic),
  nameMap = {
    cn: "cn",
    zh: "cn",
    us: "us",
    en: "us",
    quan: "quan",
    gq: "gq",
    flag: "gq",
  },
  inname = nameMap[inArg.in] || "",
  outputName = nameMap[inArg.out] || "";

// 节点国家数组（简化显示，完整数组可直接用你原来的）
const FG = ['🇭🇰','🇲🇴','🇹🇼','🇯🇵','🇰🇷','🇸🇬','🇺🇸','🇬🇧','🇫🇷','🇩🇪','🇦🇺'];
const EN = ['HK','MO','TW','JP','KR','SG','US','GB','FR','DE','AU'];
const ZH = ['香港','澳门','台湾','日本','韩国','新加坡','美国','英国','法国','德国','澳大利亚'];
const QC = ['Hong Kong','Macao','Taiwan','Japan','Korea','Singapore','United States','United Kingdom','France','Germany','Australia'];

// 保留/倍率/正则数组
const specialRegex = [ /(\d\.)?\d+×/, /IPLC|IEPL|Kern|Edge|Pro|Std|Exp|Biz|Fam|Game|Buy|Zx|LB|Game/ ];
const regexArray=[/ˣ²/, /ˣ³/, /ˣ⁴/, /ˣ⁵/, /ˣ⁶/, /ˣ⁷/, /ˣ⁸/, /ˣ⁹/, /ˣ¹⁰/, /ˣ²⁰/, /ˣ³⁰/, /ˣ⁴⁰/, /ˣ⁵⁰/, /IPLC/i, /IEPL/i, /核心/, /边缘/, /高级/, /标准/, /实验/, /商宽/, /家宽/, /游戏|game/i, /购物/, /专线/, /LB/, /cloudflare/i, /\budp\b/i, /\bgpt\b/i,/udpn\b/];
const valueArray= [ "2×","3×","4×","5×","6×","7×","8×","9×","10×","20×","30×","40×","50×","IPLC","IEPL","Kern","Edge","Pro","Std","Exp","Biz","Fam","Game","Buy","Zx","LB","CF","UDP","GPT","UDPN"];
const nameblnx = /(高倍|(?!1)2+(x|倍)|ˣ²|ˣ³|ˣ⁴|ˣ⁵|ˣ¹⁰)/i;
const namenx = /(高倍|(?!1)(0\.|\d)+(x|倍)|ˣ²|ˣ³|ˣ⁴|ˣ⁵|ˣ¹⁰)/i;

// 匹配国家关键字的对象
const rurekey = {
  德国: /(深|沪|呼|京|广|杭)德(?!.*(I|线))|法兰克福|滬德/g,
  日本: /(深|沪|呼|京|广|杭|中|辽)日(?!.*(I|线))|东京|大坂/g,
  新加坡: /狮城|(深|沪|呼|京|广|杭)新/g,
  美国: /(深|沪|呼|京|广|杭)美|波特兰|芝加哥|哥伦布|纽约|硅谷|俄勒冈|西雅图|芝加哥/g
  // 其他可继续按原脚本补充
};

let GetK = false, AMK = [];
function ObjKA(i) { GetK = true; AMK = Object.entries(i); }

// 核心处理函数
function operator(pro) {
  const Allmap = {};
  const outList = getList(outputName);
  let inputList;
  if (inname !== "") {
    inputList = [getList(inname)];
  } else {
    inputList = [ZH, FG, QC, EN];
  }

  inputList.forEach((arr) => {
    arr.forEach((value, valueIndex) => {
      Allmap[value] = outList[valueIndex];
    });
  });

  // 过滤逻辑（关键过滤已关闭 key=false，nm=true 保留未匹配）
  if (clear || nx || blnx) {
    pro = pro.filter((res) => {
      const resname = res.name;
      const shouldKeep =
        !(clear && /(套餐|到期|有效|剩余|版本|已用|过期|失联|测试)/i.test(resname)) &&
        !(nx && namenx.test(resname)) &&
        !(blnx && !nameblnx.test(resname));
      return shouldKeep;
    });
  }

  const BLKEYS = BLKEY ? BLKEY.split("+") : "";

  pro.forEach((e) => {
    let bktf = false, ens = e.name;

    // 匹配地区关键字
    Object.keys(rurekey).forEach((ikey) => {
      if (rurekey[ikey].test(e.name)) {
        e.name = e.name.replace(rurekey[ikey], ikey);
      }
    });

    // 自定义保留关键字
    if (BLKEY) {
      let BLKEY_REPLACE = "", re = false;
      BLKEYS.forEach((i) => {
        if (i.includes(">") && e.name.includes(i.split(">")[0])) {
          if (i.split(">")[1]) { BLKEY_REPLACE = i.split(">")[1]; re = true; }
        }
      });
      const retainKey = re ? BLKEY_REPLACE : BLKEYS.filter((items) => e.name.includes(items));
      e.name = e.name + (retainKey.length ? " " + retainKey.join(" ") : "");
    }

    // block-quic 配置
    if (blockquic == "on") e["block-quic"] = "on";
    else if (blockquic == "off") e["block-quic"] = "off";
    else delete e["block-quic"];

    // 处理未匹配的节点，保证 nm=true 时保留
    if (!e.name && nm) e.name = FNAME + (FNAME ? FGF : "") + ens;
  });

  jxh(pro);
  numone && oneP(pro);
  blpx && (pro = fampx(pro));
  return pro;
}

// 获取输出名称数组
function getList(arg) { switch (arg) { case 'us': return EN; case 'gq': return FG; case 'quan': return QC; default: return ZH; }}

// 序号处理函数
function jxh(e) {
  const n = e.reduce((e, n) => {
    const t = e.find((e) => e.name === n.name);
    if (t) { t.count++; t.items.push({ ...n, name: `${n.name}${XHFGF}${t.count.toString().padStart(2, "0")}` }); }
    else { e.push({ name: n.name, count: 1, items: [{ ...n, name: `${n.name}${XHFGF}01` }] }); }
    return e;
  }, []);
  const t = n.reduce((acc, e) => acc.concat(e.items), []);
  e.splice(0, e.length, ...t);
  return e;
}

// 单节点序号处理
function oneP(e) { const t = e.reduce((e, t) => { const n = t.name.replace(/[^A-Za-z0-9\u00C0-\u017F\u4E00-\u9FFF]+\d+$/, ""); if (!e[n]) e[n]=[]; e[n].push(t); return e; }, {}); for (const e in t) { if (t[e].length===1 && t[e][0].name.endsWith("01")) t[e][0].name=t[e][0].name.replace(/[^.]01/,""); } return e; }

// 排序函数
function fampx(pro) {
  const wis=[], wnout=[];
  for(const proxy of pro) specialRegex.some((regex)=>regex.test(proxy.name))?wis.push(proxy):wnout.push(proxy);
  const sps = wis.map((proxy)=>specialRegex.findIndex((regex)=>regex.test(proxy.name)));
  wis.sort((a,b)=>sps[wis.indexOf(a)]-sps[wis.indexOf(b)]||a.name.localeCompare(b.name));
  wnout.sort((a,b)=>pro.indexOf(a)-pro.indexOf(b));
  return wnout.concat(wis);
}
